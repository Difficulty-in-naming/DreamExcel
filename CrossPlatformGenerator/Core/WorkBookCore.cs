using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NPOI.OpenXml4Net.OPC;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using ScriptGenerate;
using SQLite4Unity3d;

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

        internal static HashSet<string> SupportType = new HashSet<string>
        {
            "int", "string", "bool", "long", "float", "int[]", "string[]", "long[]", "bool[]", "float[]", "enum"
        };

        internal static Dictionary<string, string> FullTypeSqliteMapping = new Dictionary<string, string>
        {
            {"int", "INTEGER"},
            {"string", "TEXT"},
            {"float", "REAL"},
            {"bool", "INTEGER"},
            {"long", "INTEGER"},
            {"int[]", "TEXT"},
            {"string[]", "TEXT"},
            {"float[]", "TEXT"},
            {"bool[]", "TEXT"},
            {"long[]", "TEXT"},
            {"enum", "INTEGER"}
        };

        public static void AnalyzerExcel(string path)
        {
            FileStream file = new FileStream(path,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
            try
            {
                
                IWorkbook wb = new XSSFWorkbook(file);
                //只有第一张表的内容才会被导出
                var sheet = wb.GetSheetAt(0);
                var table = new List<TableStruct>();
                Type newType = null;
                var haveKey = false;
                var keyType = "";
                var keyIndex = -1;
                var passColumns = new List<int>(); //跳过列
                var fileName = Path.GetFileName(path);
                var dbDirPath = Config.Instance.SaveDbPath;
                var dbFilePath = dbDirPath + fileName;
                var rowCount = sheet.LastRowNum;
                var columnCount = 0;
                for (var i = 0; i < sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null)
                        continue;
                    var cells = row.Cells;
                    if (cells.Count == 0)
                        continue;
                    columnCount = Math.Max(columnCount, cells.Count);
                    try
                    {
                        if (!Directory.Exists(dbDirPath)) Directory.CreateDirectory(dbDirPath);
                    }
                    catch (Exception e)
                    {
                        new ExcelException(e.Message);
                    }
                }

                for (var index = 0; index < columnCount; index++)
                {
                    //从1开始,第0行是策划用来写备注的地方第1行为程序使用的变量名,第2行为变量类型
                    var t1 = Convert.ToString(sheet.GetRow(NameRow).GetCell(index));
                    if (string.IsNullOrWhiteSpace(t1)) new ExcelException($"{path}表格发生错误！！！\n  单元格:{NameRow},{index} 名称不能为空");

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
                        if (keyType != "int" && keyType != "string") new ExcelException($"{path}表格发生错误！！！\n  表ID的类型不支持,可使用的类型必须为 int,string");

                        keyIndex = index;
                    }

                    table.Add(new TableStruct(t1, type, index));
                }

                if (!haveKey) new ExcelException($"{path}表格发生错误！！！\n  表格中不存在关键Key,你需要新增一列变量名为" + Key + "的变量作为键值");

                try
                {
                    var NormalTypes = new List<TypeBuilder.FieldInfo>();
                    //生成C#脚本
                    var customClass = new List<GenerateConfigTemplate>();
                    var coreClass = new GenerateConfigTemplate
                    {
                        Class = new GenerateClassTemplate {Name = fileName, Type = keyType}
                    };
                    for (var i = 0; i < table.Count; i++)
                    {
                        var t = table[i];
                        if (!SupportType.Contains(t.Type))
                        {
                            var isArray = t.Type.StartsWith("{");
                            var tuple = TableAnalyzer.GenerateCustomClass(t.Type, t.Name);
                            var newCustomType = tuple.Item1;
                            coreClass.Add(new GeneratePropertiesTemplate
                            {
                                Name = t.Name,
                                Type = newCustomType.Class.Name + (isArray ? "[]" : ""),
                                Remark = sheet.GetRow(CommentRow)?.GetCell(t.Colunm)?.ToString()
                            });
                            customClass.Add(newCustomType);
                            if (!isArray)
                                NormalTypes.Add(new TypeBuilder.FieldInfo
                                {
                                    Name = t.Name,
                                    Type = TypeBuilder.CompileResultType(tuple.Item2,
                                                                         "internal_WorkBookCore_Generate_" + t.Name)
                                });
                            else
                                NormalTypes.Add(new TypeBuilder.FieldInfo
                                {
                                    Name = t.Name,
                                    Type = TypeBuilder.CompileResultType(tuple.Item2,
                                                                         "internal_WorkBookCore_Generate_" + t.Name).MakeArrayType()
                                });
                        }
                        else
                        {
                            var core = new GeneratePropertiesTemplate {Name = t.Name, Type = t.Type, Remark = sheet.GetRow(CommentRow)?.GetCell(t.Colunm)?.ToString()};
                            if (t.Type == "enum")
                            {
                                var x = sheet.GetRow(TypeRow).GetCell(i + 1).CellComment;
                                if (x != null)
                                    core.Data = sheet.GetRow(TypeRow).GetCell(i + 1).CellComment.String.String
                                        .Replace("\r", "").Split(new[] {"\n"},
                                                                 StringSplitOptions.RemoveEmptyEntries);
                            }

                            coreClass.Add(core);
                            NormalTypes.Add(new TypeBuilder.FieldInfo
                            {
                                Name = t.Name,
                                Type = TypeHelper.ConvertStringToType(t.Type)
                            });
                        }
                    }

                    newType = TypeBuilder.CompileResultType(NormalTypes, "NewType");
                    CodeGenerate.Start(customClass, coreClass, fileName);
                }
                catch (Exception e)
                {
                    new ExcelException("生成脚本失败\n" +
                                             "可能使用了不被支持的脚本类型\n" +
                                             "当前仅支持int,int[],float,float[],bool,bool[],string,string[],long,long[]\n" +
                                             "或者自定义类的使用方法错误\n" +
                                             e);
                }

                try
                {
                    if (File.Exists(dbFilePath)) File.Delete(dbFilePath);
                }
                catch
                {
                    new ExcelException("无法写入数据库至" + dbFilePath + "请检查是否有任何应用正在使用该文件");
                }

                try
                {
                    if (Config.Instance.GeneratorType == "DB")
                        WriteDB(dbFilePath, fileName, keyType, table, columnCount, rowCount, passColumns, sheet);
                    else if (Config.Instance.GeneratorType == "Json")
                        WriteJson(dbFilePath, fileName, newType, keyIndex, columnCount, rowCount, passColumns,
                                  sheet);
                }
                catch (Exception e)
                {
                    new ExcelException("写入数据库失败\n" + e);
                }
            }
            catch (Exception e)
            {
                new ExcelException(path + " :不能被导出" + "\n" + e.Message);
            }
        }


        private static void WriteJson(string dbFilePath, string fileName, Type instanceType, int keyIndex,
            int columnCount,
            int rowCount, List<int> passColumns, ISheet sheet)
        {
            var item = new Dictionary<object, object>();
            for (var j = StartLine; j < rowCount; j++)
            {
                var instance = Activator.CreateInstance(instanceType);

                var keyCell = sheet.GetRow(j)?.GetCell(keyIndex);
                //验证数据有效性
                if (keyCell == null)
                    continue;

                for (var i = 0; i < columnCount; i++)
                    try
                    {
                        if (passColumns.Contains(i))
                            continue;
                        /*if (cells[j, i] == null)
                            continue;*/
                        var type = sheet.GetRow(NameRow).GetCell(i).ToString();
                        var property = instanceType.GetProperty(type);
                        var valueRow = sheet.GetRow(j);
                        var valueCell = valueRow?.GetCell(i);
                        if (valueCell == null)
                            continue;
                        var value = valueCell.ToString();
                        //表示内部生成的自定义类
                        if (property.PropertyType.Name.StartsWith("internal_WorkBookCore_Generate_"))
                        {
                            property.SetValue(instance, JsonConvert.DeserializeObject(value, property.PropertyType));
                        }
                        else if (property.PropertyType.IsArray)
                        {
                            var array = value.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                            var t2 = Array.CreateInstance(property.PropertyType.GetElementType(), array.Length);
                            for (var index = 0; index < array.Length; index++)
                            {
                                var node = array[index];
                                t2.SetValue(Convert.ChangeType(node, property.PropertyType.GetElementType()), index);
                            }

                            property.SetValue(instance, t2);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(value))
                                if (property.PropertyType.IsValueType)
                                    continue;
                            property.SetValue(instance, Convert.ChangeType(sheet.GetRow(j).GetCell(i).ToString(), property.PropertyType));
                        }
                    }
                    catch (Exception e)
                    {
                        new ExcelException($"单元格: {new CellReference(sheet.GetRow(j).GetCell(i)).ToString().Replace("CellReference", "")} 存在异常\n\n\n" + e);
                    }

                item[sheet.GetRow(j).GetCell(keyIndex).ToString()] = instance;
            }

            var json = JsonConvert.SerializeObject(item);
            File.WriteAllText(dbFilePath + ".json", json);
        }

        private static void WriteDB(string dbFilePath, string fileName, string keyType, List<TableStruct> table, int columnCount,
            int rowCount, List<int> passColumns, ISheet sheet)
        {
            using (var conn = new SQLiteConnection(dbFilePath + ".db", SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite))
            {
                try
                {
                    var sb = new StringBuilder();
                    var tableName = fileName;
                    var sql = new SQLiteCommand(conn);
                    sql.CommandText = "PRAGMA synchronous = OFF";
                    sql.ExecuteNonQuery();
                    //创建关键Key写入表头
                    sb.Append("create table if not exists " + tableName + " (" + Key + " " + FullTypeSqliteMapping[keyType] +
                              " PRIMARY KEY not null, ");
                    for (var n = 0; n < table.Count; n++)
                    {
                        if (table[n].Name == Key)
                            continue;
                        var t = FullTypeSqliteMapping.ContainsKey(table[n].Type);
                        var sqliteType = t ? FullTypeSqliteMapping[table[n].Type] : "TEXT";
                        sb.Append(table[n].Name + " " + sqliteType + ",");
                    }

                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");
                    sql.CommandText = sb.ToString();
                    sql.ExecuteNonQuery();
                    //准备写入表内容
                    sb.Clear();
                    conn.BeginTransaction();
                    var writeInfo = new object[columnCount];
                    for (var i = StartLine; i <= rowCount; i++)
                    {
                        var offset = 1;
                        for (var n = 1; n <= columnCount; n++)
                            try
                            {
                                if (passColumns.Contains(n))
                                {
                                    offset++;
                                    continue;
                                }

                                var property = table[n - offset];
                                var cell = Convert.ToString(sheet.GetRow(i).GetCell(n));
                                if (table.Count > n - offset)
                                {
                                    string sqliteType;
                                    if (FullTypeSqliteMapping.TryGetValue(property.Type, out sqliteType)) //常规类型可以使用这种方法直接转换
                                    {
                                        var attr = TableAnalyzer.SplitData(cell);
                                        if (property.Type == "bool")
                                            writeInfo[n - offset] = attr[0].ToUpper() == "TRUE" ? 1 : 0;
                                        else if (sqliteType != "TEXT")
                                            writeInfo[n - offset] = attr[0];
                                        else
                                            writeInfo[n - offset] = cell;
                                    }
                                    else
                                    {
                                        //自定义类型序列化
                                        writeInfo[n - 1] = cell;
                                    }
                                }
                            }
                            catch
                            {
                                new ExcelException("单元格:" + new CellReference(sheet.GetRow(i).GetCell(n)).ToString().Replace("CellReference", "") + "存在异常");
                            }

                        sb.Append("replace into " + fileName + " ");
                        sb.Append("(");
                        foreach (var node in table) sb.Append(node.Name + ",");

                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(") values (");
                        for (var index = 0; index < table.Count; index++) sb.Append("?,");

                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(")");
                        conn.CreateCommand(sb.ToString(), writeInfo).ExecuteNonQuery();
                        sb.Clear();
                    }

                    conn.Commit();
                }
                catch (Exception e)
                {
                    new ExcelException(e.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}