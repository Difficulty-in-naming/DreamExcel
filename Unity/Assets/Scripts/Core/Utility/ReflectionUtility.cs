using System;
using System.Reflection;

namespace CrossPlatformGenerator.Core.Utility
{
    public static class ReflectionUtility
    {
        public static Type GetTypeByName(this Assembly assembly,string name)
        {
            var types = assembly.GetTypes();
            foreach (var node in types)
            {
                if (node.Name.Contains(name))
                    return node;
            }

            return null;
        }
    }
}