using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ScriptGenerate;

namespace DreamExcel.Core
{
    public static class TableAnalyzer
    {
        /// <summary>
        ///     拆分格子数据
        /// </summary>
        public static string[] SplitData(string data)
        {
            data = Regex.Replace(data, "\"\"", "\"");
            var count = 0;
            var stringData = data;
/*            if (data.StartsWith("\""))
                stringData = stringData.Remove(stringData.Length - 1, 1).Remove(0, 1);*/
            var sb = new StringBuilder();
            var dataList = new List<string>();
            for (var i = 0; i < stringData.Length; i++)
            {
                var c = stringData[i];
                if (c == '\"')
                {
                    count++;
                }
                else if (count % 2 == 0 && c == ',')
                {
                    count = 0;
                    dataList.Add(sb.ToString());
                    sb = new StringBuilder();
                    continue;
                }
                else if (count == 0 && c == '\n')
                    continue;
                sb.Append(c);
                if (i == stringData.Length - 1)
                    dataList.Add(sb.ToString());
            }
            return dataList.ToArray();
        }

        public static bool CheckType(string str)
        {
            return WorkBookCore.SupportType.Contains(str);

        }

        public static Tuple<GenerateConfigTemplate,List<TypeBuilder.FieldInfo>> GenerateCustomClass(string t,string n = "")
        {
            var split = t.TrimStart('{').TrimEnd('}').Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var newCustomType = new GenerateConfigTemplate { Class = new GenerateClassTemplate { Name = n + "Data" } };
            List<TypeBuilder.FieldInfo> Types = new List<TypeBuilder.FieldInfo>();
            
            for (int j = 0; j < split.Length; j++)
            {
                var content = split[j];
                string name = "", type = "";
                var indexOfType = content.IndexOf("[");
                name = content.Substring(0, indexOfType);
                type = content.Replace(name, "");
                if (type.EndsWith("]"))
                {
                    type = type.Substring(0, type.Length - 1);
                }
                if (type.StartsWith("["))
                {
                    type = type.Remove(0, 1);
                }
                if (!CheckType(type))
                {
                    throw new ExcelException("表名:" + name + "中的类型定义有误,不能使用int,int[],string,string[],bool,bool[],float,float[]以外的类型" +
                                             "\n如果需要定义扩展类型请使用这种格式:Name[string];Id[int]" +
                                             "\n如果需要定义扩展类型数组请使用这种格式:{Name[string];Id[int]}");
                }
                newCustomType.Add(new GeneratePropertiesTemplate { Name = name, Type = type });
                Types.Add(new TypeBuilder.FieldInfo {Name = name, Type = TypeHelper.ConvertStringToType(type)});
            }
            return new Tuple<GenerateConfigTemplate, List<TypeBuilder.FieldInfo>>(newCustomType,Types);
        }
    }
}

