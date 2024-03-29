using System.Collections.Generic;
namespace CrossPlatformGenerator
{
    public class ScriptTemplate
    {
        public class VariableDefine
        {
            public string Type;
            public string Name;
            public string Comment;
            public List<string> Attribute;
            public VariableDefine(string type, string name, string comment,List<string> attribute = null)
            {
                Type = type;
                Name = name;
                Comment = comment;
                Attribute = attribute ?? new List<string>();
            }
        }

        public class EnumValue
        {
            public string Name;
            public int? Value;

            public EnumValue(string name, int? value = null)
            {
                Name = name;
                Value = value;
            }
        }
        public class EnumDefine
        {
            public string Name;
            public List<EnumValue> Values = new List<EnumValue>();
            public bool IsFlag;
            public EnumDefine(string name)
            {
                Name = name;
            }
        }
        
        public class ClassDefine
        {
            public string KeyType;
            public string Name;
            public List<EnumDefine> Enum = new List<EnumDefine>();
            public List<VariableDefine> Variables = new List<VariableDefine>();
            public List<string> Attribute = new List<string>();
        }
        
        public List<ClassDefine> CustomClass = new List<ClassDefine>();
        public ClassDefine CoreClass = new ClassDefine();
    }
}