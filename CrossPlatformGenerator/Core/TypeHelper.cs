using System;

namespace DreamExcel.Core
{
    public static class TypeHelper
    {
        public static Type ConvertStringToType(string type)
        {
            type = type.ToLower();
            if (type == "int" || type == "system.int")
                return typeof(int);
            else if (type == "float" || type == "system.single" || type == "system.float")
                return typeof(float);
            else if (type == "string" || type == "system.string")
                return typeof(string);
            else if (type == "bool" || type == "boolean" || type == "system.bool" || type == "system.boolean")
                return typeof(bool);
            else if (type == "long" || type == "system.long")
                return typeof(long);
            else if (type == "int[]" || type == "system.int[]")
                return typeof(int[]);
            else if (type == "float[]" || type == "system.single[]" || type == "system.float[]")
                return typeof(float[]);
            else if (type == "string[]" || type == "system.string[]")
                return typeof(string[]);
            else if (type == "bool[]" || type == "boolean[]" || type == "system.bool[]" || type == "system.boolean[]")
                return typeof(bool[]);
            else if (type == "long[]" || type == "system.long[]")
                return typeof(long[]);
            else if (type == "enum" || type == "system.enum")
                return typeof(int);
            else if (type == "enum[]" || type == "system.enum[]")
                return typeof(int[]);
            return null;
        } 
    }
}