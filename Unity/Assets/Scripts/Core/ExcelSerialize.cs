using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CrossPlatformGenerator;
using CrossPlatformGenerator.Core;
using DreamLib.Editor.Unity.Extensition;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DreamExcel.Core
{
    public class ExcelSerialize
    {
        private CSVRecords mRecords;
        
        public class CSVRecords
        {
            public readonly string[][] Rows;
            private int mKeyIndex = -1;
            public int KeyIndex
            {
                get => mKeyIndex == -1 ? GetIdIndex() : mKeyIndex;
                private set => mKeyIndex = value;
            }
            public int RowCount { get; }
            public int ColumnsCount { get; }
            private ISheet Sheet { get; }
            public CSVRecords(string path)
            {
                var newFilePath = "./" + Guid.NewGuid() + ".xlsx";
                Console.WriteLine(new FileInfo(path));
                File.Copy(path,newFilePath);
                using var stream = new FileStream(newFilePath, FileMode.Open);
                stream.Position = 0;
                XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                Sheet = xssWorkbook.GetSheetAt(0);
                RowCount = Sheet.LastRowNum;
                Rows = new string[RowCount + 1][];
                IRow headerRow = Sheet.GetRow(Config.NameRow);
                ColumnsCount = headerRow.LastCellNum;
                for (int i = Sheet.FirstRowNum; i <= RowCount; i++)
                {
                    IRow row = Sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    Rows[i] = new string[ColumnsCount];
                    for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell != null)
                        {
                            var cellString = cell.ToString();
                            if (!string.IsNullOrEmpty(cellString) && !string.IsNullOrWhiteSpace(cellString))
                            {
                                if (j >= 0 && j < Rows[i].Length)
                                {
                                    if (cell.CellType == CellType.Formula)
                                        Rows[i][j] = cell.StringCellValue;
                                    else
                                        Rows[i][j] = cellString;
                                }
                            }
                        }
                    }
                }
                stream.Close();
                File.Delete(newFilePath);
            }

            public string GetCell(int row,int cell)
            {
                return Sheet.GetRow(row).GetCell(cell).ToString();
            }
            
            public int GetIdIndex()
            {
                if (mKeyIndex >= 0)
                    return mKeyIndex;
                var array = Rows[Config.NameRow];
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == Config.DataKey)
                        return mKeyIndex = i;
                }
                return mKeyIndex;
            }

            public string GetIdType()
            {
                return GetType(GetIdIndex());
            }
            
            public string GetTypeByName(string name,bool ignoreCase)
            {
                var array = Rows[Config.NameRow];
                for (int i = 0; i < array.Length; i++)
                {
                    if (string.Equals(array[i],name,ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                        return Rows[Config.TypeRow][i];
                }
                return null;
            }

            public string GetName(int index)
            {
                return Rows[Config.NameRow][index].Trim();
            }

            public string GetComment(int index)
            {
                return Rows[Config.CommentRow][index];
            }
            
            public string GetType(int index)
            {
                return Rows[Config.TypeRow][index].Trim();
            }

            public string GetOwner(int index)
            {
                return Rows[Config.OwnerRow][index].Trim();
            }
        }


        public void AnalyzerExcel(string path)
        {
            var excelName = Path.GetFileNameWithoutExtension(path);
            if (excelName.EndsWith(Config.Info.FileSuffix))
            {
                string fileName = Path.GetFileNameWithoutExtension(path);
                if(!string.IsNullOrEmpty(Config.Info.FileSuffix))
                {
                    fileName = fileName.Replace(Config.Info.FileSuffix, "");
                }

                try
                {
                    mRecords = new CSVRecords(path);

                    if (mRecords.GetIdIndex() == -1)
                        throw new ExcelException($"{path}表格发生错误！！！\n  表格中不存在关键Key,你需要新增一列变量名为" + Config.DataKey + "的变量作为键值(注意区分大小写！！)");
                    var scriptString = GenerateScript(fileName);
                    Config.DeleteDB(fileName);
                    try
                    {
                        if (Config.Info.GeneratorType == "Json")
                        {
                            if (Config.GetDBPath(fileName).Count == 0)
                            {
                                mRecords.WriteJson(Config.ExePath + "/" + fileName);
                            }
                            else
                            {
                                foreach (var node in Config.GetDBPath(fileName))
                                {
                                    mRecords.WriteJson(node);
                                }
                            }
                        }
                        else if (Enum.TryParse<BytesFlag>(Config.Info.GeneratorType,out var byteFlag))
                        {
                            if (Config.GetDBPath(fileName).Count == 0)
                            {
                                mRecords.WriteBytes(Config.ExePath + "/" + fileName,scriptString,fileName,byteFlag);
                            }
                            else
                            {
                                foreach (var node in Config.GetDBPath(fileName))
                                {
                                    mRecords.WriteBytes(node,scriptString,fileName,byteFlag);
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
            
        }

        private string GenerateScript(string fileName)
        {
            try
            {
                var scriptTemplate = new ScriptTemplate();
                //生成C#脚本
                for (var i = 0; i < mRecords.ColumnsCount; i++)
                {
                    var t = mRecords.GetType(i);
                    var n = mRecords.GetName(i);
                    if (string.IsNullOrEmpty(n))
                        continue;
                    if (n.StartsWith("*"))
                        continue;
                    if (string.IsNullOrEmpty(t))
                        continue;
                    var comment = mRecords.GetComment(i);
                    var customClass = t.Split(Config.SplitChar2, StringSplitOptions.RemoveEmptyEntries);
                    if (customClass.Length > 1) //发现这个类型是自定义类型
                    {
                        bool isArray = t.StartsWith("{");
                        scriptTemplate.CoreClass.Variables.Add(new ScriptTemplate.VariableDefine(n + "Data" + (isArray ? "[]" : ""), n, comment));
                        var define = new ScriptTemplate.ClassDefine();
                        foreach (var node in customClass)
                        {
                            var str = node.TrimStart('{').TrimEnd('}');
                            var startIndex = str.IndexOf("[", StringComparison.Ordinal);
                            var endIndex = str.LastIndexOf("]", StringComparison.Ordinal);
                            var name = str.Substring(0, startIndex);
                            var type = str.Substring(startIndex + 1, endIndex - startIndex - 1);
                            define.Variables.Add(new ScriptTemplate.VariableDefine(type, name, null));
                        }
                        define.Name = n + "Data";
                        scriptTemplate.CustomClass.Add(define);
                    }
                    else
                    {
                        if (t.Equals("Enum", StringComparison.OrdinalIgnoreCase) || t.Equals("Enum[Flags]", StringComparison.OrdinalIgnoreCase))
                        {
                            var define = new ScriptTemplate.EnumDefine(n + "Enum");
                            if (t.Equals("Enum[Flags]", StringComparison.OrdinalIgnoreCase))
                                define.IsFlag = true;
                            HashSet<string> hash = new HashSet<string>();
                            for (var index = Config.StartLine; index < mRecords.RowCount; index++)
                            {
                                var row = mRecords.Rows[index];
                                if (row == null)
                                    continue;
                                var value = row[i];
                                if (hash.Add(value) && !string.IsNullOrEmpty(value))
                                    define.Values.Add(new ScriptTemplate.EnumValue(value));
                            }

                            scriptTemplate.CoreClass.Enum.Add(define);
                            scriptTemplate.CoreClass.Variables.Add(new ScriptTemplate.VariableDefine(n + "Enum", n, comment));
                        }
                        else
                        {
                            List<string> attribute = new List<string>();
                            var match = Regex.Matches(n, @"\[.*?\]");
                            var list = new List<string>();
                            foreach (Match node in match)
                            {
                                list.Add(node.Value);
                            }
                            attribute.AddRange(list);
                            var name = Regex.Replace(n, @"\[.*?\]", "");
                            scriptTemplate.CoreClass.Variables.Add(new ScriptTemplate.VariableDefine(t, name, comment, attribute));
                        }
                    }
                }

                scriptTemplate.CoreClass.KeyType = mRecords.GetIdType();
                scriptTemplate.CoreClass.Name = fileName;
                if(Config.Info.GeneratorType == BytesFlag.MessagePack.ToString())
                    scriptTemplate.CoreClass.Attribute.Add($"[MessagePack.MessagePackObject]");
                else if(Config.Info.GeneratorType == BytesFlag.Protobuf.ToString())
                    scriptTemplate.CoreClass.Attribute.Add($"[ProtoBuf.ProtoContract]");
                else if (Config.Info.GeneratorType == BytesFlag.MasterMemory.ToString())
                {
                }
                else if (Config.Info.GeneratorType == BytesFlag.MemoryPack.ToString())
                    scriptTemplate.CoreClass.Attribute.Add($"[MemoryPack.MemoryPackable]");
                Generate generate;
                if (File.Exists(Config.ExePath + Path.DirectorySeparatorChar + fileName + ".txt"))
                {
                    generate = new Generate(Config.ExePath + Path.DirectorySeparatorChar + fileName + ".txt");
                }
                else
                {
                    generate = new Generate(Config.ExePath + Path.DirectorySeparatorChar + "GenerateTemplate.txt");
                }

                var coreClass = generate.Tree.GetChild("CoreClass");
                coreClass.GetChild("Class")?.Replace(scriptTemplate.CoreClass.Name);
                coreClass.GetChild("KeyType")?.Replace(scriptTemplate.CoreClass.KeyType);
                coreClass.GetChild("Attribute")?.Replace(string.Join("\n", scriptTemplate.CoreClass.Attribute));
                foreach (var node in scriptTemplate.CustomClass)
                {
                    var customClass = coreClass.GetChild("CustomClass");
                    if (customClass != null)
                    {
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

                        coreClass.Add("CustomClass", customClass);
                    }
                }
                
                for (int i = 0; i < scriptTemplate.CoreClass.Variables.Count; i++)
                {
                    var variable = scriptTemplate.CoreClass.Variables[i];
                    if(Config.Info.GeneratorType == BytesFlag.MessagePack.ToString())
                        variable.Attribute.Add($"[MessagePack.Key({i})]");
                    else if(Config.Info.GeneratorType == BytesFlag.Protobuf.ToString())
                        variable.Attribute.Add($"[ProtoBuf.ProtoMember({i + 1})]");
                    if (Config.Info.GeneratorType == BytesFlag.MasterMemory.ToString())
                    {
                        if(variable.Name == Config.DataKey)
                            variable.Attribute.Add($"[PrimaryKey]");
                    }
                    if (coreClass.GetChild("Field") != null)
                    {
                        var field = coreClass.GetChild("Field").Clone();
                        field.GetChild("Type").Replace(variable.Type);
                        field.GetChild("Name").Replace(variable.Name);
                        field.GetChild("Attribute")?.Replace(string.Join("\n", variable.Attribute));
                        coreClass.Add("Field", field);
                    }
                    if (coreClass.GetChild("Property") != null)
                    {
                        var property = coreClass.GetChild("Property").Clone();
                        property.GetChild("Comment").Replace(variable.Comment);
                        property.GetChild("Type").Replace(variable.Type);
                        property.GetChild("Name").Replace(variable.Name);
                        property.GetChild("Attribute")?.Replace(string.Join("\n", variable.Attribute));
                        coreClass.Add("Property", property);
                    }

                }

                for (int i = 0; i < scriptTemplate.CoreClass.Enum.Count; i++)
                {
                    var enumList = scriptTemplate.CoreClass.Enum[i];
                    if (coreClass.GetChild("Enum") != null)
                    {
                        var classEnum = coreClass.GetChild("Enum").Clone();
                        classEnum.GetChild("Name").Replace(enumList.Name);
                        if (enumList.IsFlag)
                            classEnum.GetChild("Flag").Replace("[Flags]");
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
                }

                var keyType = mRecords.GetTypeByName("Key", true);
                if (!string.IsNullOrEmpty(keyType))
                {
                    var keyPart = coreClass.GetChild("HasKey").Clone();
                    keyPart.GetChild("Class").Replace(scriptTemplate.CoreClass.Name);
                    keyPart.GetChild("KeyType").Replace(keyType);
                    coreClass.Add("HasKey", keyPart);
                }
                generate.Tree.Add("CoreClass", coreClass);
                var scriptString = Config.FormatCode(generate.ToString());
                Config.WriteScript(fileName + Config.Info.CodeSuffix,scriptString );
                return scriptString;
            }
            catch (Exception e)
            {
                throw new ExcelException(e.ToString());
            }
        }
    }
}