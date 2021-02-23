using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Nett;

namespace DreamExcel.Core
{
    public class Config
    {
        public static string ExePath
        {
            get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }

        public static Data Info
        {
            get
            {
                var configPath = ExePath + "/Config.txt";
                if (!File.Exists(configPath))
                    configPath = CurrentPath + "/Config.txt";
                return Toml.ReadFile<Data>(configPath);
            }
        }

        private static string CurrentPath => AppDomain.CurrentDomain.BaseDirectory;

        public static string ScriptTemplatePath
        {
            get
            {
                var configPath = ExePath + "/GenerateTemplate.txt";
                if (File.Exists(configPath))
                    return configPath;
                return CurrentPath + "/GenerateTemplate.txt";
            }
        }

        public class Data
        {
            public string GeneratorType { get; set; }
            public string ScriptNameSpace { get; set; }
            public string FileSuffix { get; set; }
            public string EnumSuffix { get; set; }
            public string[] SaveDbPath { get; set; }
            public string[] SaveScriptPath { get; set; }
        }

        public static void WriteScript(string file, string content)
        {
            foreach (var node in Info.SaveScriptPath)
            {
                if (Path.IsPathRooted(node))
                {
                    var path = node + Path.DirectorySeparatorChar + file;
                    var dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    File.WriteAllText(path, content);
                }
                else
                {
                    var path = ExePath + Path.DirectorySeparatorChar + node + Path.DirectorySeparatorChar + file;
                    var dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    File.WriteAllText(path, content);
                }
            }
        }

        public static List<string> GetDBPath(string fileName)
        {
            List<string> s = new List<string>();
            foreach (var node in Info.SaveDbPath)
            {
                var file = fileName;
                if (Path.IsPathRooted(node))
                {
                    file = node + Path.DirectorySeparatorChar + file;
                    var dir = Path.GetDirectoryName(file);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                }
                else
                {
                    file = ExePath + Path.DirectorySeparatorChar + node + Path.DirectorySeparatorChar + file;
                    var dir = Path.GetDirectoryName(file);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                }

                s.Add(file);
            }

            return s;
        }

        public static void DeleteDB(string file)
        {
            foreach (var node in Info.SaveDbPath)
            {
                var path = file;
                if (Path.IsPathRooted(node))
                    path = node + Path.DirectorySeparatorChar + path;
                else
                    path = ExePath + Path.DirectorySeparatorChar + node + Path.DirectorySeparatorChar + path;
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch
                {
                    throw new ExcelException("无法写入数据库至" + path + "请检查是否有任何应用正在使用该文件");
                }
            }
        }
    }
}