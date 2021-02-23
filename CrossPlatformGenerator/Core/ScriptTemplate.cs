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

            public VariableDefine(string type, string name, string comment)
            {
                Type = type;
                Name = name;
                Comment = comment;
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
        }
        
        public List<ClassDefine> CustomClass = new List<ClassDefine>();
        public ClassDefine CoreClass = new ClassDefine();
    }
}