using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using CrossPlatformGenerator;
using DreamLib.Editor.Unity.Extensition;
using Hocon;
using Hocon.Json;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace DreamExcel.Core
{
    public class CSVSerialize
    {
        /// <summary>
        ///     第X行开始才是正式数据
        /// </summary>
        private const int StartLine = 3;

        /// <summary>
        ///     关键Key值,这个值在Excel表里面必须存在
        /// </summary>
        private const string Key = "Id";

        /// <summary>
        ///     类型的所在行
        /// </summary>
        public const int TypeRow = 2;

        /// <summary>
        ///     注释内容的所在行
        /// </summary>
        public const int CommentRow = 0;
        
        /// <summary>
        ///     变量名称的所在行
        /// </summary>
        public const int NameRow = 1;

        public class CSVRecords
        {
            public DataTable CsvData { get; }
            public DataRowCollection Rows => CsvData.Rows;
            public DataColumnCollection Columns => CsvData.Columns;

            private int mKeyIndex = -1;
            public int KeyIndex
            {
                get => mKeyIndex == -1 ? GetIdIndex() : mKeyIndex;
                private set => mKeyIndex = value;
            }

            public int RowCount => CsvData.Rows.Count;
            public CSVRecords(string path)
            {
                CsvData = new DataTable();
                using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (TextFieldParser csvReader = new TextFieldParser(fileStream,new UTF8Encoding(true)))
                {
                    csvReader.SetDelimiters(new string[] {","});
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields;
                    bool tableCreated = false;
                    while (tableCreated == false)
                    {
                        colFields = csvReader.ReadFields();
                        for (var index = 0; index < colFields.Length; index++)
                        {
                            CsvData.Columns.Add(index.ToString());
                        }

                        CsvData.Rows.Add(colFields);

                        tableCreated = true;
                    }
                    
                    while (!csvReader.EndOfData)
                    {
                        CsvData.Rows.Add(csvReader.ReadFields());
                    }
                }
            }

            public int GetIdIndex()
            {
                if (mKeyIndex >= 0)
                    return mKeyIndex;
                var array = Rows[NameRow].ItemArray;
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].ToString() == Key)
                        return mKeyIndex = i;
                }
                return mKeyIndex;
            }

            public string GetIdType()
            {
                return GetType(GetIdIndex());
            }
            
            public string GetTypeByName(string name,bool ingoreCase)
            {
                var array = Rows[NameRow].ItemArray;
                for (int i = 0; i < array.Length; i++)
                {
                    if (string.Equals(array[i].ToString(),name,ingoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                        return Rows[TypeRow][i].ToString();
                }
                return null;
            }

            public string GetName(int index)
            {
                return Rows[NameRow][index].ToString();
            }

            public string GetComment(int index)
            {
                return Rows[CommentRow][index].ToString();
            }
            
            public string GetType(int index)
            {
                return Rows[TypeRow][index].ToString();
            }
        }

        private CSVRecords mRecords;

        public void AnalyzerExcel(string path)
        {
            //var destFile = Config.ExePath + "/" + Path.GetFileNameWithoutExtension(path) + "Temp";
            //Console.WriteLine($"Copy源文件{path} To {destFile}");
            //File.Copy(path, destFile, true);
            var fileName = Path.GetFileNameWithoutExtension(path)?.Replace(Config.Info.FileSuffix, "");
            try
            {
                mRecords = new CSVRecords(path);
                string keyType = "";
                if (mRecords.GetIdIndex() == -1)
                    throw new ExcelException($"{path}表格发生错误！！！\n  表格中不存在关键Key,你需要新增一列变量名为" + Key + "的变量作为键值(注意区分大小写！！)");
                GenerateScript(fileName);
                Config.DeleteDB(fileName);
                try
                {
                    if (Config.Info.GeneratorType == "Json")
                    {
                        if (Config.GetDBPath(fileName).Count == 0)
                        {
                            WriteJson(Config.ExePath + "/" + fileName);
                        }
                        else
                        {
                            foreach (var node in Config.GetDBPath(fileName))
                            {
                                WriteJson(node);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ExcelException("写入数据库失败\n" + e);
                }
            }
            catch(Exception e)
            {
                throw new ExcelException(path + " :不能被导出" + "\n" + e);
            }
        }

        private void GenerateScript(string fileName)
        {
            try
            {
                var scriptTemplate = new ScriptTemplate();
                //生成C#脚本
                for (var i = 0; i < mRecords.Columns.Count; i++)
                {
                    var t = mRecords.GetType(i);
                    var n = mRecords.GetName(i);
                    if (n.StartsWith("*"))
                        continue;
                    var comment = mRecords.GetComment(i);
                    var customClass = t.Split(";", StringSplitOptions.RemoveEmptyEntries);
                    if (customClass.Length > 1) //发现这个类型是自定义类型
                    {
                        bool isArray = t.StartsWith("{");
                        scriptTemplate.CoreClass.Variables.Add(new ScriptTemplate.VariableDefine(n + "Data" + (isArray ? "[]" : ""), n, comment));
                        var define = new ScriptTemplate.ClassDefine();
                        foreach (var node in customClass)
                        {
                            var str = node.TrimStart('{').TrimEnd('}');
                            var startIndex = str.IndexOf("[");
                            var endIndex = str.LastIndexOf("]");
                            var name = str.Substring(0, startIndex);
                            var type = str.Substring(startIndex + 1, endIndex - startIndex - 1);
                            define.Variables.Add(new ScriptTemplate.VariableDefine(type, name, null));
                        }

                        define.Name = n + "Data";
                        scriptTemplate.CustomClass.Add(define);
                    }
                    else
                    {
                        if (t.Equals("Enum",StringComparison.OrdinalIgnoreCase))
                        {
                            var define = new ScriptTemplate.EnumDefine(n + "Enum");
                            HashSet<string> hash = new HashSet<string>();
                            for (var index = StartLine; index < mRecords.Rows.Count; index++)
                            {
                                DataRow row = mRecords.Rows[index];
                                var value = row[i].ToString();
                                if (hash.Add(value))
                                    define.Values.Add(new ScriptTemplate.EnumValue(value));
                            }

                            scriptTemplate.CoreClass.Enum.Add(define);
                            scriptTemplate.CoreClass.Variables.Add(new ScriptTemplate.VariableDefine(n + "Enum", n, comment));
                        }
                        else
                            scriptTemplate.CoreClass.Variables.Add(new ScriptTemplate.VariableDefine(t, n, comment));
                    }
                }

                scriptTemplate.CoreClass.KeyType = mRecords.GetIdType();
                scriptTemplate.CoreClass.Name = fileName;
                Generate generate;
                if (File.Exists(Config.ExePath + Path.DirectorySeparatorChar + fileName + ".txt"))
                {
                    generate = new Generate(Config.ExePath + Path.DirectorySeparatorChar + fileName + ".txt");
                }
                else
                {
                    generate = new Generate(Config.ExePath + Path.DirectorySeparatorChar + "GenerateTemplate.txt");
                }

                foreach (var node in scriptTemplate.CustomClass)
                {
                    var customClass = generate.Tree.GetChild("CustomClass");
                    customClass.GetChild("Class").Replace(node.Name);
                    foreach (var variable in node.Variables)
                    {
                        var field = customClass.GetChild("Field").Clone();
                        field.GetChild("Name").Replace(variable.Name);
                        field.GetChild("Type").Replace(variable.Type);
                        customClass.Add("Field", field);

                        var property = customClass.GetChild("Property").Clone();
                        property.GetChild("Name").Replace(variable.Name);
                        property.GetChild("Type").Replace(variable.Type);
                        customClass.Add("Property", property);
                    }

                    generate.Tree.Add("CustomClass", customClass);
                }

                var coreClass = generate.Tree.GetChild("CoreClass");
                coreClass.GetChild("Class").Replace(scriptTemplate.CoreClass.Name);
                coreClass.GetChild("KeyType").Replace(scriptTemplate.CoreClass.KeyType);
                for (int i = 0; i < scriptTemplate.CoreClass.Variables.Count; i++)
                {
                    var variable = scriptTemplate.CoreClass.Variables[i];
                    var field = coreClass.GetChild("Field").Clone();
                    field.GetChild("Type").Replace(variable.Type);
                    field.GetChild("Name").Replace(variable.Name);
                    coreClass.Add("Field", field);
                    var property = coreClass.GetChild("Property").Clone();
                    property.GetChild("Comment").Replace(variable.Comment);
                    property.GetChild("Type").Replace(variable.Type);
                    property.GetChild("Name").Replace(variable.Name);
                    coreClass.Add("Property", property);
                }

                for (int i = 0; i < scriptTemplate.CoreClass.Enum.Count; i++)
                {
                    var enumList = scriptTemplate.CoreClass.Enum[i];
                    var classEnum = coreClass.GetChild("Enum").Clone();
                    classEnum.GetChild("Name").Replace(enumList.Name);
                    foreach (var node in enumList.Values)
                    {
                        var enumNested = classEnum.GetChild("Nested").Clone();
                        enumNested.GetChild("Key").Replace(node.Name);
                        if (node.Value.HasValue)
                            enumNested.GetChild("Value").Replace(" = " + node.Value);
                        classEnum.Add("Nested", enumNested);
                    }
                    coreClass.Add("Enum", classEnum);
                }

                var keyPart = coreClass.GetChild("HasKey").Clone();
                keyPart.GetChild("Class").Replace(scriptTemplate.CoreClass.Name);
                keyPart.GetChild("KeyType").Replace(mRecords.GetTypeByName("Key", true));
                coreClass.Add("HasKey", keyPart);
                
                generate.Tree.Add("CoreClass", coreClass);
                Config.WriteScript(fileName + ".cs", generate.ToString());
            }
            catch (Exception e)
            {
                throw new ExcelException(e.ToString());
            }
        }
        
        private void WriteJson(string dbFilePath)
        {
            JsonSerializerSettings currentSettings = JsonConvert.DefaultSettings?.Invoke() ?? new JsonSerializerSettings();
            currentSettings.Converters.Add(new StringEnumConverter());
            JsonConvert.DefaultSettings = () => currentSettings;
            //var item = new Dictionary<object, object>();
            var jObject = new JObject();
            for (var j = StartLine; j < mRecords.RowCount; j++)
            {
                JObject jValue = new JObject();
                var valueRow = mRecords.Rows[j];
                var key = valueRow[mRecords.GetIdIndex()];
                var column = valueRow.ItemArray;
                for (var i = 0; i < column.Length; i++)
                {
                    try
                    {
                        var name = mRecords.GetName(i);
                        var type = mRecords.GetType(i);
                        if (name.StartsWith("*"))
                            continue;
                        var valueCell = column[i];
                        if (valueCell != null && !string.IsNullOrEmpty(valueCell.ToString()))
                        {
                            if (type == "int" || type == "long" || type == "short" || type == "uint" || type == "ushort" || type == "byte")
                                jValue.Add(name, long.Parse(valueCell.ToString()));
                            else if (type == "bool")
                                jValue.Add(name, BoolParse(valueCell.ToString()));
                            else if (type == "bool[]" || type.Contains("List<bool>"))
                                jValue.Add(name, new JArray(valueCell.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(BoolParse).ToArray()));
                            else if (type == "ulong")
                                jValue.Add(name, ulong.Parse(valueCell.ToString()));
                            else if (type == "float" || type == "double")
                                jValue.Add(name, double.Parse(valueCell.ToString()));
                            else if (type == "string")
                                jValue.Add(name, valueCell.ToString());
                            else if (type == "int[]" || type == "long[]" || type == "short[]" || type == "uint[]" || type == "ushort[]" || type == "byte[]" || type.Contains("List<long>") || type.Contains("List<short>") || type.Contains("List<uint>") || type.Contains("List<ushort>") || type.Contains("List<byte>"))
                                jValue.Add(name, new JArray(valueCell.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()));
                            else if (type == "ulong[]" || type.Contains("List<ulong>"))
                                jValue.Add(name, new JArray(valueCell.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray()));
                            else if (type == "float[]" || type == "double[]" || type.Contains("List<float>") || type.Contains("List<double>"))
                                jValue.Add(name, new JArray(valueCell.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray()));
                            else if (type.Contains("string[]") || type.Contains("List<string>"))
                                jValue.Add(name, new JArray(valueCell.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)));
                            else
                            {
                                try
                                {
                                    var value = valueCell.ToString();
                                    var parse = HoconParser.Parse(value);
                                    var token = parse.ToJToken();
                                    jValue.Add(name, token);
                                }
                                catch
                                {
                                    jValue.Add(name, valueCell.ToString());
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var cell = new CellReference(j, i);
                        throw new ExcelException($"单元格:{cell.FormatAsString()} 格式错误");
                    }
                }

                jObject.Add(key.ToString(), jValue);
            }

            File.WriteAllText(dbFilePath + ".json", jObject.ToString());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("导出Json文件| " + new DirectoryInfo(dbFilePath).FullName + ".json");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static bool BoolParse(string value)
        {
            if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
                return false;
            else if (int.TryParse(value, out int i))
            {
                if (i > 0)
                    return true;
                else
                    return false;
            }

            return false;
        }
    }
}