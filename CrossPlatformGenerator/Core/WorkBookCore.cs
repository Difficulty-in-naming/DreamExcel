/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CrossPlatformGenerator;
using DreamLib.Editor.Unity.Extensition;
using Hocon;
using Hocon.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace DreamExcel.Core
{
    public class WorkBookCore
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
        ///     变量名称的所在行
        /// </summary>
        public const int NameRow = 1;

        /// <summary>
        ///     注释名称的所在行
        /// </summary>
        public const int CommentRow = 0;
        
        public static void AnalyzerExcel(string path)
        {
            //var destFile = Config.ExePath + "/" + Path.GetFileNameWithoutExtension(path) + "Temp";
            //Console.WriteLine($"Copy源文件{path} To {destFile}");
            //File.Copy(path, destFile, true);
            try
            {
                using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                try
                {
                    IWorkbook wb = new XSSFWorkbook(stream);
                    //只有第一张表的内容才会被导出
                    var sheet = wb.GetSheetAt(wb.ActiveSheetIndex);
                    if (sheet.SheetName.StartsWith("&"))
                        return;
                    var table = new List<TableStruct>();
                    Type newType = null;
                    var scriptTemplate = new ScriptTemplate();
                    var haveKey = false;
                    var keyType = "";
                    var keyIndex = -1;
                    var passColumns = new List<int>(); //跳过列
                    var fileName = Path.GetFileNameWithoutExtension(path)?.Replace(Config.Info.FileSuffix, "");
                    fileName = sheet.SheetName;
                    var rowCount = sheet.LastRowNum;
                    var columnCount = 0;
                    for (var i = 0; i <= rowCount; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                            continue;
                        var cells = row.Cells;
                        if (cells.Count == 0)
                            continue;
                        columnCount = Math.Max(columnCount, cells.Count);
                    }

                    try
                    {
                        for (var index = 0; index < columnCount; index++)
                        {
                            //从1开始,第0行是策划用来写备注的地方第1行为程序使用的变量名,第2行为变量类型
                            var cell = sheet.GetRow(NameRow).GetCell(index);
                            var t1 = Convert.ToString(cell);

                            if (string.IsNullOrWhiteSpace(t1)) throw new ExcelException($"{path}表格发生错误！！！\n  单元格:{new CellReference(NameRow, index).FormatAsString()} 不能为空,如果这一列没有东西.那么就选中这一列然后删除");

                            if (t1.StartsWith("*"))
                            {
                                passColumns.Add(index);
                                continue;
                            }

                            var type = sheet.GetRow(TypeRow).GetCell(index).ToString();
                            if (t1 == Key)
                            {
                                haveKey = true;
                                keyType = type;
                                if (keyType != "int" && keyType != "string") throw new ExcelException($"{path}表格发生错误！！！\n  表ID的类型不支持,可使用的类型必须为 int,string");

                                keyIndex = index;
                            }

                            var comment = sheet.GetRow(CommentRow).GetCell(index).ToString();

                            table.Add(new TableStruct(t1, type, comment, index));
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }


                    if (!haveKey) throw new ExcelException($"{path}表格发生错误！！！\n  表格中不存在关键Key,你需要新增一列变量名为" + Key + "的变量作为键值(注意区分大小写！！)");

                    try
                    {
                        //生成C#脚本
                        for (var i = 0; i < table.Count; i++)
                        {
                            var t = table[i];
                            var customClass = t.Type.Split(";", StringSplitOptions.RemoveEmptyEntries);
                            if (customClass.Length > 1) //发现这个类型是自定义类型
                            {
                                bool isArray = t.Type.StartsWith("{");
                                scriptTemplate.CoreClass.Variables.Add(new ScriptTemplate.VariableDefine(t.Name + "Data" + (isArray ? "[]" : ""), t.Name, t.Comment));
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

                                define.Name = t.Name + "Data";
                                scriptTemplate.CustomClass.Add(define);
                            }
                            else
                            {
                                if (t.Type == "enum")
                                {
                                    var x = sheet.GetRow(TypeRow).GetCell(t.Colunm).CellComment;
                                    if (x != null)
                                    {
                                        var define = new ScriptTemplate.EnumDefine(t.Name + "Enum");
                                        var e = x.String.String.Replace("\r", "").Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var node in e)
                                        {
                                            var maohaoIndex = node.IndexOf(":");
                                            var nameStartIndex = node.LastIndexOf('[') + 1;
                                            var name = node.Substring(nameStartIndex, node.LastIndexOf(']') - nameStartIndex);
                                            if (maohaoIndex > 0)
                                            {
                                                string value = node.Substring(0, maohaoIndex);
                                                define.Values.Add(new ScriptTemplate.EnumValue(name, int.Parse(value)));
                                            }
                                            else
                                                define.Values.Add(new ScriptTemplate.EnumValue(name));
                                        }

                                        scriptTemplate.CoreClass.Enum.Add(define);
                                    }

                                    scriptTemplate.CoreClass.Variables.Add(new ScriptTemplate.VariableDefine(t.Name + "Enum", t.Name, t.Comment));
                                }
                                else
                                    scriptTemplate.CoreClass.Variables.Add(new ScriptTemplate.VariableDefine(t.Type, t.Name, t.Comment));
                            }
                        }

                        scriptTemplate.CoreClass.KeyType = keyType;
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

                        generate.Tree.Add("CoreClass", coreClass);
                        Config.WriteScript(fileName + ".cs", generate.ToString());
                    }
                    catch (Exception e)
                    {
                        throw new ExcelException(e.ToString());
                    }

                    Config.DeleteDB(fileName);
                    try
                    {
                        if (Config.Info.GeneratorType == "Json")
                        {
                            if (Config.GetDBPath(fileName).Count == 0)
                            {
                                WriteJson(Config.ExePath + "/" + fileName, newType, keyIndex, columnCount, rowCount, passColumns,
                                    sheet);
                            }
                            else
                            {
                                foreach (var node in Config.GetDBPath(fileName))
                                {
                                    WriteJson(node, newType, keyIndex, columnCount, rowCount, passColumns,
                                        sheet);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ExcelException("写入数据库失败\n" + e);
                    }
                }
                catch (Exception e)
                {
                    throw new ExcelException(path + " :不能被导出" + "\n" + e.Message);
                }
            }
            finally
            {
                //File.Delete(destFile);
            }

            //Console.WriteLine($"导出Excel成功");
        }

        private static void WriteJson(string dbFilePath, Type instanceType, int keyIndex, int columnCount, int rowCount, List<int> passColumns, ISheet sheet)
        {
            JsonSerializerSettings currentSettings = JsonConvert.DefaultSettings?.Invoke() ?? new JsonSerializerSettings();
            currentSettings.Converters.Add(new StringEnumConverter());
            JsonConvert.DefaultSettings = () => currentSettings;
            //var item = new Dictionary<object, object>();
            var jObject = new JObject();
            for (var j = StartLine; j <= rowCount; j++)
            {
                JObject jValue = new JObject();
                var valueRow = sheet.GetRow(j);
                if (valueRow == null)
                    continue;
                var key = valueRow.GetCell(keyIndex);
                if (key == null)
                    continue;
                for (var i = 0; i < columnCount; i++)
                {
                    try
                    {
                        var name = sheet.GetRow(NameRow).GetCell(i).ToString();
                        var type = sheet.GetRow(TypeRow).GetCell(i).ToString();
                        if (name.StartsWith("*"))
                            continue;
                        var valueCell = valueRow.GetCell(i);
                        if (valueCell != null && !string.IsNullOrEmpty(valueCell.ToString()))
                        {
                            if (type == "int" || type == "long" || type == "short" || type == "uint" || type == "ushort" || type == "byte")
                                jValue.Add(name, long.Parse(valueCell.ToString()));
                            else if (type == "bool")
                                jValue.Add(name, BoolParse(valueCell.ToString()));
                            else if (type == "bool[]")
                                jValue.Add(name, new JArray(valueCell.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(BoolParse).ToArray()));
                            else if (type == "ulong")
                                jValue.Add(name, ulong.Parse(valueCell.ToString()));
                            else if (type == "float" || type == "double")
                                jValue.Add(name, valueCell.NumericCellValue);
                            else if (type == "string")
                                jValue.Add(name, valueCell.ToString());
                            else if (type == "int[]" || type == "long[]" || type == "short[]" || type == "uint[]" || type == "ushort[]" || type == "byte[]")
                                jValue.Add(name, new JArray(valueCell.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()));
                            else if (type == "ulong[]")
                                jValue.Add(name, new JArray(valueCell.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray()));
                            else if (type == "float[]" || type == "double[]")
                                jValue.Add(name, new JArray(valueCell.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray()));
                            else if (type.Contains("string[]"))
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
}*/