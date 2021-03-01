using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Nett;

namespace DreamExcel.Core
{
    public class Config
    {
        public static string ExePath
        {
            get;
            set;
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

        public static string[] CSVDelimiters
        {
            get
            {
                return Info.CSVDelimiters;
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
            public string[] CSVDelimiters { get; set; }
        }

        public static void WriteScript(string file, string content)
        {
            try
            {
                foreach (var node in Info.SaveScriptPath)
                {
                    if (Path.IsPathRooted(node))
                    {
                        var path = node + Path.DirectorySeparatorChar + file;
                        var dir = Path.GetDirectoryName(path);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        File.WriteAllText(path, FormatCode(content));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("导出脚本文件| " + new FileInfo(path).FullName);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        var path = ExePath + Path.DirectorySeparatorChar + node + Path.DirectorySeparatorChar + file;
                        var dir = Path.GetDirectoryName(path);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        File.WriteAllText(path, FormatCode(content));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("导出脚本文件| " + new FileInfo(path).FullName);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("导出脚本失败");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static string FormatCode(string source)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            var workSpace = new AdhocWorkspace();
            workSpace.AddSolution(
                SolutionInfo.Create(SolutionId.CreateNewId("formatter"), 
                    VersionStamp.Default)
            );

            var formatter = Formatter.Format(tree.GetCompilationUnitRoot(), workSpace);
            return tree.GetCompilationUnitRoot().NormalizeWhitespace().ToFullString();
            //return temp.ToFullString();
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