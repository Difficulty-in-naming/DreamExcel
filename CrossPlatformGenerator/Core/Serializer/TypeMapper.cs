using System;
using System.Collections.Generic;

namespace CrossPlatformGenerator.Core
{
    public class TypeMapper
    {
        public static Type DoJob(string type)
        {
            switch (type)
            {
                case "int" : return typeof(int);
                case "uint" : return typeof(uint);
                case "long" : return typeof(long);
                case "short" : return typeof(short);
                case "ushort" : return typeof(ushort);
                case "byte" : return typeof(byte);
                case "bool" : return typeof(bool);
                case "bool[]" : return typeof(bool[]);
                case "List<bool>" : return typeof(List<bool>);
                case "ulong" : return typeof(ulong);
                case "float" : return typeof(float);
                case "double" : return typeof(double);
                case "int[]" : return typeof(int[]);
                case "long[]" : return typeof(long[]);
                case "short[]" : return typeof(short[]);
                case "uint[]" : return typeof(uint[]);
                case "ushort[]" : return typeof(ushort[]);
                case "List<long>" : return typeof(List<long>);
                case "List<uint>" : return typeof(List<uint>);
                case "List<short>" : return typeof(List<short>);
                case "List<ushort>" : return typeof(List<ushort>);
                case "ulong[]" : return typeof(ulong[]);
                case "List<ulong>" : return typeof(List<ulong>);
                case "float[]" : return typeof(float[]);
                case "double[]" : return typeof(double[]);
                case "List<float>" : return typeof(List<float>);
                case "List<double>" : return typeof(List<double>);
                case "string[]" : return typeof(string[]);
                case "HashSet<string>" : return typeof(HashSet<string>);
                case "IEnumerable<string>" : return typeof(IEnumerable<string>);
                default: return null;
            }
        }
    }
}