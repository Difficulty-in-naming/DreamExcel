using System;
using System.Collections.Generic;
using System.Text;
using ScriptGenerate;

namespace DreamExcel.Core
{
    public static class ConvertStringToBinary
    {
        internal static Dictionary<string, Func<string[], object>> ValueConverter = new Dictionary<string, Func<string[], object>>
        {
            {"System.Int32", str => str.Length > 0 ? System.Convert.ToInt32(str[0]) : 0},
            {"System.String", str => str.Length > 0 ? str[0] : ""},
            {"System.Boolean", str => str.Length > 0 && System.Convert.ToBoolean(str[0])},
            {"System.Single", str => str.Length > 0 ? System.Convert.ToSingle(str[0]) : 0},
            {"System.Int64", str => str.Length > 0 ? System.Convert.ToInt64(str[0]) : 0},
            {"System.Int32[]", str => str.Length > 0 ? Array.ConvertAll(str, int.Parse) : new int[0]},
            {"System.Int64[]", str => str.Length > 0 ? Array.ConvertAll(str, long.Parse) : new long[0]},
            {"System.Boolean[]", str => str.Length > 0 ? Array.ConvertAll(str, bool.Parse) : new bool[0]},
            {"System.String[]", str => str.Length > 0 ? str : new string[0]},
            {"System.Single[]", str => str.Length > 0 ? Array.ConvertAll(str, float.Parse) : new float[0]}
        };

        public static void Convert(string context, GenerateConfigTemplate template)
        {
            bool isArray = context[0] == '[' && context[context.Length - 1] == ']';
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < context.Length; i++)
            {
                char c = context[i];
/*                if (c == '{')//开始截取
                {
                    sb.Append()
                }*/
            }
        }

        public static void ConvertArray(string context, string type)
        {
            context = context.Remove(0, 1);
            context = context.Remove(context.Length - 2, 1);
            var split = context.Split(new string[]{","},StringSplitOptions.RemoveEmptyEntries);
            List<byte> lb = new List<byte>();
            if (type == "System.Single[]")
            {
                var value = (float[])ValueConverter[type](split);
                for (int i = 0; i < value.Length; i++)
                {
                    var bytes = BitConverter.GetBytes(value[i]);
                    for(int j = 0;j < 4;j++)
                        lb.Add(bytes[j]);
                }
            }
            else if (type == "System.Int32[]")
            {
                var value = (int[])ValueConverter[type](split);
                for (int i = 0; i < value.Length; i++)
                {
                    var bytes = BitConverter.GetBytes(value[i]);
                    for (int j = 0; j < 4; j++)
                        lb.Add(bytes[j]);
                }
            }
            else if (type == "System.String[]")
            {
                for (int i = 0; i < split.Length; i++)
                {
                    var length = BitConverter.GetBytes(split[i].Length);
                    for (int j = 0; j < 4; j++)
                        lb.Add(length[j]);
                    var bytes = Encoding.UTF8.GetBytes(split[i]);
                    for (int j = 0; j < bytes.Length; j++)
                        lb.Add(bytes[j]);
                }
            }
            else if (type == "System.Boolean[]")
            {
                var value = (bool[])ValueConverter[type](split);
                for (int i = 0; i < value.Length; i++)
                {
                    var bytes = BitConverter.GetBytes(value[i]);
                    lb.Add(bytes[0]);
                }
            }
            else if (type == "System.Int64[]")
            {
                var value = (int[])ValueConverter[type](split);
                for (int i = 0; i < value.Length; i++)
                {
                    var bytes = BitConverter.GetBytes(value[i]);
                    for (int j = 0; j < 8; j++)
                        lb.Add(bytes[j]);
                }
            }
        }
    }
}
