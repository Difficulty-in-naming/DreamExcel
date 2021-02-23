using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DreamLib.Editor.Unity.Extensition;
using ScriptGenerate;

namespace DreamExcel.Core
{
    public static class CodeGenerate
    {
        private static Action<Generate.Info, Serializer> Defalut(List<GenerateConfigTemplate> customClass, GenerateConfigTemplate core)
        {
            Action<Generate.Info, Serializer> action = (info, g) =>
            {
                if (info.PlaceHolder == "CustomClass")
                {
                    for (var i = 0; i < customClass.Count; i++)
                    {
                        var custom = customClass[i];
                        g.SetReplace("Class", custom.Class.Name);
                        g.SetReplace("Attribute", custom.Class.Attribute);
                        var properties = custom.Properties;
                        if (g.BeginGroup("NestedField"))
                        {
                            for (var j = 0; j < properties.Count; j++)
                            {
                                g.SetReplace("Name", properties[j].Name);
                                g.SetReplace("Type", properties[j].Type);
                                g.Apply();
                            }
                            g.EndGroup();
                        }

                        if (g.BeginGroup("NestedProperty"))
                        {
                            for (var j = 0; j < properties.Count; j++)
                            {
                                g.SetReplace("Name", properties[j].Name);
                                g.SetReplace("Type", properties[j].Type);
                                g.Apply();
                            }
                            g.EndGroup();
                        }
                        g.Apply();
                    }
                }
                if (info.PlaceHolder == "CoreClass")
                {
                    g.SetReplace("Class", core.Class.Name);
                    g.SetReplace("Attribute", core.Class.Attribute);
                    g.SetReplace("KeyType", core.Class.Type);
                    if (core.Class.Type == "string")
                    {
                        g.SetReplace("Search", "string.Format(\"SELECT * FROM " + core.Class.Name + " WHERE Id = '{0}'\",id)");
                    }
                    else
                    {
                        g.SetReplace("Search", "string.Format(\"SELECT * FROM " + core.Class.Name + " WHERE Id = {0}\",id)");
                    }
                    var properties = core.Properties;

                    if (g.BeginGroup("Enum"))
                    {
                        foreach (var node in core.Properties)
                        {
                            if (node.Type == "enum")
                            {
                                string[] split = (string[])node.Data;
                                g.SetReplace("Name", node.Name + Config.Info.EnumSuffix);
                                if (g.BeginGroup("Nested"))
                                {
                                    for (int i = 0; i < split.Length; i++)
                                    {
                                        var kv = split[i].Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                        Utility.CheckCondition(() => kv.Length > 1, "解析枚举出错");
                                        var match = Regex.Match(kv[1], @"\[(.*)\]");
                                        if (match.Success)
                                        {
                                            g.SetReplace("Key", Regex.Match(kv[1], @"\[(.*)\]").Groups[1].Value);
                                            g.SetReplace("Value", kv[0]);
                                            g.Apply();
                                        }
                                    }
                                    g.EndGroup();
                                }
                                g.Apply();
                            }
                        }
                        g.EndGroup();
                    }

                    if (g.BeginGroup("Field"))
                    {
                        for (var i = 0; i < properties.Count; i++)
                        {
                            g.SetReplace("Name", properties[i].Name);
                            if (properties[i].Type == "enum")
                            {
                                g.SetReplace("Type", properties[i].Name + Config.Info.EnumSuffix);
                            }
                            else
                                g.SetReplace("Type", properties[i].Type);
                            g.Apply();
                        }
                        g.EndGroup();
                    }

                    if (g.BeginGroup("Property"))
                    {
                        for (var i = 0; i < properties.Count; i++)
                        {
                            g.SetReplace("Name", properties[i].Name);
                            g.SetReplace("Comment", properties[i].Remark);
                            if (properties[i].Type == "enum")
                            {
                                g.SetReplace("Type", properties[i].Name + Config.Info.EnumSuffix);
                            }
                            else
                                g.SetReplace("Type", properties[i].Type);
                            g.Apply();
                        }
                        g.EndGroup();
                    }
                    g.Apply();
                }
                if (info.PlaceHolder == "ScriptableObject")
                {
                    g.SetReplace("Class", core.Class.Name);
                    g.Apply();
                }
            };
            return action;
        }

        public static string Start(Action<Generate.Info, Serializer> action,string fileName)
        {
            var path = Config.ScriptTemplatePath;
            string g = new Generate(path, action).StartWrite();
            g = Generate.FormatScript(g);
            Config.WriteScript(fileName + ".cs",g);
            return g;
        }

        public static string Start(List<GenerateConfigTemplate> customClass, GenerateConfigTemplate core, string fileName)
        {
            var action = Defalut(customClass, core);
            return Start(action,fileName);
        }
    }
}
