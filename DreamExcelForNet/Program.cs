using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DreamExcel.Core;

namespace DreamExcelForNet
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    return;
                if (args[0] == "Gen")
                {
                    Batch.Gen(args[1]);
                }
                else if (args[0] == "StartServer")
                {
                    Batch.StartServer(args[1]);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        public static class Batch 
        {
            public static void Gen(string path)
            {
                var attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var extension = new List<string> {".xlsx", "xls"};
                    foreach (var node in Directory.GetFiles(path, "*.*").Where(f => extension.Contains(Path.GetExtension(f))))
                    {
                        WorkBookCore.AnalyzerExcel(node);
                        Console.WriteLine($"导出{node}成功");
                    }
                }
                else
                {
                    WorkBookCore.AnalyzerExcel(path);
                    Console.WriteLine($"导出{path}成功");
                }
            }
            
            public static void StartServer(string path)
            {
                path = Path.GetFullPath(path);
                Console.WriteLine("开启服务器成功" + "路径:" + path);
                FileSystemWatcher fsw = new FileSystemWatcher(path);
                fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Security | NotifyFilters.Size | NotifyFilters.CreationTime;
//                fsw.Path = path;
                fsw.EnableRaisingEvents = true;
                fsw.Changed+=FswOnChanged;
                fsw.IncludeSubdirectories = true;
            }

            private static void FswOnChanged(object sender, FileSystemEventArgs e)
            {
                string strFileExt = Path.GetExtension(e.FullPath);
                string fileName = Path.GetFileName(e.FullPath);
                if (strFileExt == ".xlsx" || strFileExt == ".xls")
                {
                    if (fileName.StartsWith("~"))
                        return;
                    if (!Path.GetFileNameWithoutExtension(fileName).EndsWith(Config.Info.FileSuffix))
                        return;
                    WorkBookCore.AnalyzerExcel(e.FullPath);
                    Console.WriteLine($"导出{e.FullPath}成功");
                }
            }
        }
    }
}