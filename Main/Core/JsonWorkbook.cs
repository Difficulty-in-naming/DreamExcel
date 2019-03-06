using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace DreamExcel.Core
{
    public class JsonWorkbook
    {
        private static Range mLastCell = null;
        private static Application mApp;
        public const string TableName = "Json Convert";
        private static List<TypeBuilder.FieldInfo> mTypeinfo;
        private static Type mType;
        private static bool Saved;
        private static Worksheet mOrignSheet;
        private static bool mIsArray;
        public JsonWorkbook(Application app)
        {
            mApp = app;
            Saved = true;
            mApp.OnKey("+^e","EnterJsonEdit");
        }

        public static void EnterJsonEdit()
        {
            var app = WorkBookCore.App;

            if (((Worksheet)app.ActiveWorkbook.ActiveSheet).Name == TableName)
                return;
            mOrignSheet = (Worksheet) app.ActiveWorkbook.ActiveSheet;
            if (mLastCell != null)
            {
                    
            }

            var s = WorksheetExists(app);

            if (!Saved && s != null)
            {
                var result = MessageBox.Show("没有保存文档.可能会导致当前修改的Json数据丢失,是否丢弃", "", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            var activeSheet = (Worksheet) app.ActiveSheet;
            Saved = false;
            mLastCell = app.ActiveCell;
            int column = mLastCell.Column;
            var name = ((dynamic)activeSheet.UsedRange[WorkBookCore.NameRow,column]).Text;
            var type = ((dynamic)activeSheet.UsedRange[WorkBookCore.TypeRow,column]).Text;
            mIsArray = type.StartsWith("{");

            if (WorkBookCore.SupportType.Contains(type))
            {
                return;
            }
            else
            {
                if(s == null)
                {
                    s = (Worksheet)app.Worksheets.Add();
                    s.Name = TableName;
                }
                else
                {
                    s.Delete();
                    s = (Worksheet)app.Worksheets.Add();
                    s.Name = TableName;
                }

                var tuple = TableAnalyzer.GenerateCustomClass(type);
                mTypeinfo = (List<TypeBuilder.FieldInfo>)tuple.Item2;

                for (var index = 0; index < mTypeinfo.Count; index++)
                {
                    var node = mTypeinfo[index];
                    ((Range) s.Cells.Item[1, index + 1]).Value = node.Name;
                    ((Range) s.Cells.Item[2, index + 1]).Value = TypeHelper.ConvertTypeToString(node.Type);
                }

                int count = 3;
                mType = TypeBuilder.CompileResultType(mTypeinfo, "internal_WorkBookCore_Generate_" + (string) name);
                if (mIsArray)
                {
                    var target = (JArray)Deserialize(mLastCell.Value.ToString());
                    foreach (var item in target.Children())
                    {
                        foreach (var prop in item.Children<JProperty>())
                        {
                            var index = mTypeinfo.FindIndex(t1=>t1.Name == prop.Name);
                            if (index == -1)
                                throw new ExcelException("错误");
                            ((Range) s.Cells.Item[count, index + 1]).Value = prop.Value;
                        }
                        count++;
                    }
                }
                else
                {
                    var target = (JObject)Deserialize(mLastCell.Value.ToString());
                    foreach (var prop in target.Children<JProperty>())
                    {
                        var index = mTypeinfo.FindIndex(t1=>t1.Name == prop.Name);
                        if (index == -1)
                            throw new ExcelException("错误");
                        ((Range) s.Cells.Item[count, index + 1]).Value = prop.Value;
                    }
                }
            }
        }

        private static Worksheet WorksheetExists(Application app)
        {
            Worksheet s = null;

            foreach (Worksheet sheet in app.Sheets)
            {
                if (sheet.Name == TableName)
                {
                    s = sheet;
                    break;
                }
            }

            return s;
        }

        private static object Deserialize(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            try
            {
                return JsonConvert.DeserializeObject(value);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }
        
        private static string Serialize(object value)
        {
            if (value == null)
                return "";
            return JsonConvert.SerializeObject(value);
        }
        
        public void Save(Workbook wb, bool b, ref bool r)
        {
            try
            {
                Saved = true;
                var activeSheet = (Worksheet) wb.ActiveSheet;
                if (activeSheet.Name != TableName)
                    return;
                Range usedRange = activeSheet.UsedRange;
                var rowCount = usedRange.Rows.Count;
                var columnCount = usedRange.Columns.Count;
                List<object> item = new List<object>();
                for (int j = 3; j <= rowCount; j++)
                {
                    var instance = Activator.CreateInstance(mType);

                    for (int i = 1; i <= columnCount; i++)
                    {
                        try
                        {
                            var type = ((Range)activeSheet.Cells[1, i]).Value.ToString();
                            var property = mType.GetProperty(type);
                            if (property == null)
                                continue;
                            string t1 = Convert.ToString(((Range)activeSheet.Cells[j, i]).Value.ToString());
                            {
                                if (string.IsNullOrEmpty(t1))
                                {
                                    if (property.PropertyType.IsValueType)
                                        continue;
                                }

                                property.SetValue(instance, Convert.ChangeType(((Range)activeSheet.Cells[j, i]).Value.ToString(), property.PropertyType));
                            }
                        }
                        catch (Exception e)
                        {
                            throw new ExcelException($"单元格: {((Range) activeSheet.Cells[j, i]).Address} 存在异常\n\n\n" + e);
                        }
                    }

                    item.Add(instance);
                }

                if (mIsArray)
                    mLastCell.Value = Serialize(item);
                else
                    mLastCell.Value = Serialize(item.FirstOrDefault());
                //返回最初的表格
                mOrignSheet.Activate();
                mLastCell.Select();
                activeSheet.Delete();
            }
            catch (Exception e)
            {
                throw new ExcelException(e.ToString());
            }
        }
    }
}