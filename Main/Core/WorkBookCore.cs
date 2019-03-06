using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using ScriptGenerate;
using SQLite4Unity3d;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace DreamExcel.Core
{
    public class WorkBookCore: IExcelAddIn
    {
        /// <summary>
        /// 绑定
        /// </summary>
        public static Application App = (Application)ExcelDnaUtil.Application;
        /// <summary>
        /// 第X行开始才是正式数据
        /// </summary>
        public const int StartLine = 4;
        /// <summary>
        ///     关键Key值,这个值在Excel表里面必须存在
        /// </summary>
        private const string Key = "Id";

        /// <summary>
        /// 类型的所在行
        /// </summary>
        public const int TypeRow = 3;
        /// <summary>
        /// 变量名称的所在行
        /// </summary>
        public const int NameRow = 2;
        /// <summary>
        /// 注释名称的所在行
        /// </summary>
        public const int CommentRow = 1;
        /// <summary>
        /// 特殊类 
        /// </summary>
        public JsonWorkbook JsonWorkbook;
        internal static HashSet<string> SupportType = new HashSet<string>
        {
            "int","string","bool","long","float","int[]","string[]","long[]","bool[]","float[]","enum",
        };

        internal static Dictionary<string, string> FullTypeSqliteMapping = new Dictionary<string, string>
        {
            {"int","INTEGER" },
            {"string","TEXT" },
            {"float","REAL" },
            {"bool","INTEGER" },
            {"long","INTEGER" },
            {"int[]","TEXT"},
            {"string[]","TEXT"},
            {"float[]","TEXT"},
            {"bool[]","TEXT"},
            {"long[]","TEXT"},
            {"enum","INTEGER" }
        };
        
        private void Workbook_BeforeSave(Workbook wb,bool b, ref bool r)
        {
            if (wb.IsVaildWorkBook()) return;
            var activeSheet = (Worksheet) wb.ActiveSheet;
            if (activeSheet.Name == JsonWorkbook.TableName)
                return;
            var fileName = Path.GetFileNameWithoutExtension(wb.Name)?.Replace(Config.Instance.FileSuffix, "");
            var dbDirPath = Config.Instance.SaveDbPath;
            var dbFilePath = dbDirPath + fileName;
            try
            {
                if (!Directory.Exists(dbDirPath))
                {
                    Directory.CreateDirectory(dbDirPath);
                }
            }
            catch(Exception e)
            {
                throw new ExcelException(e.Message);
            }

            Range usedRange = activeSheet.UsedRange;
            var rowCount = usedRange.Rows.Count;
            var columnCount = usedRange.Columns.Count;
            List<TableStruct> table = new List<TableStruct>();
            Type newType;
            bool haveKey = false;
            string keyType = "";
            int keyIndex = -1;
            object[,] cells = (object[,])usedRange.Value2;
            List<int> passColumns = new List<int>(); //跳过列
            for (var index = 1; index < columnCount + 1; index++)
            {
                //从1开始,第0行是策划用来写备注的地方第1行为程序使用的变量名,第2行为变量类型
                string t1 = Convert.ToString(cells[NameRow, index]);
                if (string.IsNullOrWhiteSpace(t1))
                {
                    var cell = ((Range)usedRange.Cells[NameRow, index]).Address;
                    throw new ExcelException("单元格:" + cell + "名称不能为空");
                }
                if (t1.StartsWith("*"))
                {
                    passColumns.Add(index);
                    continue;
                }
                string type = Convert.ToString(cells[TypeRow, index]);
                if (t1 == Key)
                {
                    haveKey = true;
                    keyType = type;
                    if (keyType!="int" && keyType != "string")
                    {
                        throw new ExcelException("表ID的类型不支持,可使用的类型必须为 int,string");
                    }
                    keyIndex = index;
                }

                table.Add(new TableStruct(t1, type, index));
            }

            if (!haveKey)
            {
                throw new ExcelException("表格中不存在关键Key,你需要新增一列变量名为" + Key + "的变量作为键值");
            }
            try
            {
                List<TypeBuilder.FieldInfo> normalTypes = new List<TypeBuilder.FieldInfo>();
                //生成C#脚本
                var customClass = new List<GenerateConfigTemplate>();
                var coreClass = new GenerateConfigTemplate {Class = new GenerateClassTemplate {Name = fileName, Type = keyType}};
                for (int i = 0; i < table.Count; i++)
                {
                    var t = table[i];
                    if (!SupportType.Contains(t.Type))
                    {
                        bool isArray = t.Type.StartsWith("{");
                        var tuple = TableAnalyzer.GenerateCustomClass(t.Type, t.Name);
                        var newCustomType = tuple.Item1; 
                        coreClass.Add(new GeneratePropertiesTemplate {Name = t.Name, Type = newCustomType.Class.Name + (isArray ? "[]" : ""),Remark = ((Range)usedRange[CommentRow, t.Colunm]).Text.ToString()});
                        customClass.Add(newCustomType);
                        if(!isArray)
                            normalTypes.Add(new TypeBuilder.FieldInfo{Name = t.Name,Type = TypeBuilder.CompileResultType(tuple.Item2,"internal_WorkBookCore_Generate_" + t.Name)});
                        else
                            normalTypes.Add(new TypeBuilder.FieldInfo{Name = t.Name,Type = TypeBuilder.CompileResultType(tuple.Item2,"internal_WorkBookCore_Generate_" + t.Name).MakeArrayType()});
                    }
                    else
                    {
                        GeneratePropertiesTemplate core = new GeneratePropertiesTemplate {Name = t.Name, Type = t.Type,Remark = ((Range)usedRange[CommentRow, t.Colunm]).Text.ToString()};
                        if (t.Type == "enum")
                        {
                            //不知道为什么无法获取到注释但是加2却可以了
                            Range x = ((Range) usedRange[TypeRow, i + 2]);
                            if (x.Comment != null)
                                core.Data = ((Range) usedRange[TypeRow, i + 2]).Comment.Text().Replace("\r", "").Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
                        }
                        coreClass.Add(core);
                        normalTypes.Add(new TypeBuilder.FieldInfo{Name = t.Name,Type = TypeHelper.ConvertStringToType(t.Type)});
                    }
                }
                newType = TypeBuilder.CompileResultType(normalTypes,"NewType");
                CodeGenerate.Start(customClass, coreClass, fileName);
            }
            catch (Exception e)
            {
                throw new ExcelException("生成脚本失败\n" +
                                         "可能使用了不被支持的脚本类型\n" +
                                         "当前仅支持int,int[],float,float[],bool,bool[],string,string[],long,long[]\n" +
                                         "或者自定义类的使用方法错误\n" + 
                                         e);
            }
            try
            {
                if (File.Exists(dbFilePath))
                {
                    File.Delete(dbFilePath);
                }
            }
            catch
            {
                throw new ExcelException("无法写入数据库至" + dbFilePath + "请检查是否有任何应用正在使用该文件");
            }

            try
            {
                if (Config.Instance.GeneratorType == "DB")
                {
                    WriteDB(dbFilePath, fileName, keyType, table, columnCount, rowCount, passColumns, cells, usedRange);
                }
                else if (Config.Instance.GeneratorType == "Json")
                {
                    WriteJson(dbFilePath, fileName, newType,keyIndex, columnCount, rowCount, passColumns, cells,
                        usedRange);
                }
            }
            catch (Exception e)
            {
                throw new ExcelException("写入数据库失败\n" + e);
            }
        }

        private static void WriteJson(string dbFilePath, string fileName, Type instanceType,int keyIndex,
            int columnCount,
            int rowCount, List<int> passColumns, object[,] cells, Range usedRange)
        {
            Dictionary<object,object> item = new Dictionary<object,object>();
            var properties = instanceType.GetProperties().ToList();

            for (int j = StartLine; j <= rowCount; j++)
            {
                var instance = Activator.CreateInstance(instanceType);
                //验证数据有效性
                if (cells[j, keyIndex] == null)
                    continue;
                
                for(int i = 1; i<= columnCount;i++)
                {
                    try
                    {
                        if (passColumns.Contains(i))
                            continue;
                        /*if (cells[j, i] == null)
                            continue;*/
                        var type = (string) cells[NameRow, i];
                        var property = properties.Find(t => t.Name == type);
                        if (property == null)
                            continue;
                        string t1 = Convert.ToString(cells[j, i]);
                        //表示内部生成的自定义类
                        if (property.PropertyType.Name.StartsWith("internal_WorkBookCore_Generate_"))
                        {
                            property.SetValue(instance, JsonConvert.DeserializeObject(t1, property.PropertyType));
                        }
                        else if (property.PropertyType.IsArray)
                        {
                            var array = t1.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
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
                            if (string.IsNullOrEmpty(t1))
                            {
                                if (property.PropertyType.IsValueType)
                                    continue;
                            }
                            property.SetValue(instance, Convert.ChangeType(cells[j, i], property.PropertyType));
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ExcelException($"单元格: {((Range)usedRange.Cells[j, i]).Address} 存在异常\n\n\n" + e );
                    }
                }
                item[cells[j,keyIndex]] = instance;
            }
            string json = JsonConvert.SerializeObject(item);
            File.WriteAllText(dbFilePath + ".json",json);
        }
        
        private static void WriteDB(string dbFilePath, string fileName, string keyType, List<TableStruct> table, int columnCount,
            int rowCount, List<int> passColumns, object[,] cells, Range usedRange)
        {
            using (var conn = new SQLiteConnection(dbFilePath + ".db", SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    var tableName = fileName;
                    SQLiteCommand sql = new SQLiteCommand(conn) {CommandText = "PRAGMA synchronous = OFF"};
                    sql.ExecuteNonQuery();
                    //创建关键Key写入表头
                    sb.Append("create table if not exists " + tableName + " (" + Key + " " + FullTypeSqliteMapping[keyType] +
                              " PRIMARY KEY not null, ");
                    for (int n = 0; n < table.Count; n++)
                    {
                        if (table[n].Name == Key)
                            continue;
                        var t = FullTypeSqliteMapping.ContainsKey(table[n].Type);
                        string sqliteType = t ? FullTypeSqliteMapping[table[n].Type] : "TEXT";
                        sb.Append(table[n].Name + " " + sqliteType + ",");
                    }

                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(")");
                    sql.CommandText = sb.ToString();
                    sql.ExecuteNonQuery();
                    //准备写入表内容
                    sb.Clear();
                    conn.BeginTransaction();
                    object[] writeInfo = new object[columnCount];
                    for (int i = StartLine; i <= rowCount; i++)
                    {
                        int offset = 1;
                        for (var n = 1; n <= columnCount; n++)
                        {
                            try
                            {
                                if (passColumns.Contains(n))
                                {
                                    offset++;
                                    continue;
                                }

                                var property = table[n - offset];
                                string cell = Convert.ToString(cells[i, n]);
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
                                throw new Exception("单元格:" + ((Range) usedRange.Cells[i, n]).Address + "存在异常");
                            }
                        }

                        sb.Append("replace into " + fileName + " ");
                        sb.Append("(");
                        foreach (var node in table)
                        {
                            sb.Append(node.Name + ",");
                        }

                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(") values (");
                        for (var index = 0; index < table.Count; index++)
                        {
                            sb.Append("?,");
                        }

                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(")");
                        conn.CreateCommand(sb.ToString(), writeInfo).ExecuteNonQuery();
                        sb.Clear();
                    }

                    conn.Commit();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        public void AutoOpen()
        {
            JsonWorkbook = new JsonWorkbook(App);
            App.WorkbookBeforeSave += JsonWorkbook.Save;
            App.WorkbookBeforeSave += Workbook_BeforeSave;
            App.WorkbookActivate+= AppOnWorkbookActivate;
            App.SheetChange += OnAppOnSheetChange;
        }

        public void AutoClose()
        {
        }

        private void AppOnWorkbookActivate(Workbook wb)
        {
            OnAppOnSheetChange(wb.ActiveSheet, null);
        }

        private void OnAppOnSheetChange(object sh, Range obj)
        {
            ((Worksheet) sh).PivotTableAfterValueChange += (table, range) => throw new ExcelException("TableAfterValue");
            //((Worksheet) sh).BeforeDoubleClick += JsonWorkbook.OnSelectionChange;
        }

        public void AddSupportType(string str)
        {
            var support = str.Split(':');
            SupportType.Add(support[0]);
            FullTypeSqliteMapping.Add(support[0],support[1]);
        }
    }
}



