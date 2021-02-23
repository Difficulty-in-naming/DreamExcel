using System;
using System.IO;
using DreamExcel.Core;

namespace CrossPlatformGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                if (args[0] == "Gen")
                {
                    Batch.Gen(args[1]);
                }
                else if (args[0] == "StartServer")
                {
                    Batch.StartServer(args[1]);
                }
            }
            else
            {
                if (args[1] == "Gen")
                {
                    Batch.Gen(args[2]);
                }
                else if (args[1] == "StartServer")
                {
                    Batch.StartServer(args[2]);
                }
            }
            Console.Read();
        }

        public static class Batch 
        {
            private static DateTime _lastTimeFileWatcherEventRaised;
            private static FileSystemWatcher _watcher;
            public static void Gen(string path)
            {
                /*var attr = File.GetAttributes(path);
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
                }*/
            }
            
            public static void StartServer(string path)
            {
                Console.WriteLine("开启服务器成功");
                path = Path.GetDirectoryName(path);
                _watcher = new FileSystemWatcher(path);
                GC.KeepAlive(_watcher);
                _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Security | NotifyFilters.Size | NotifyFilters.CreationTime;
//                fsw.Path = path;
                _watcher.Filter = "*.csv";
                _watcher.Changed+=FswOnChanged;
                _watcher.Error += FswOnError;
                _watcher.IncludeSubdirectories = true;

                _watcher.EnableRaisingEvents = true;
            }

            private static void FswOnError(object sender, ErrorEventArgs e)
            {
                Console.WriteLine("Watcher_Error: " + e.GetException().Message);
            }

            private static void FswOnChanged(object sender, FileSystemEventArgs e)
            {
                if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    string fileName = Path.GetFileName(e.FullPath);
                    if( DateTime.Now.Subtract (_lastTimeFileWatcherEventRaised).TotalMilliseconds < 800 )
                    {
                        return;
                    }
                    _lastTimeFileWatcherEventRaised = DateTime.Now;
                    try
                    {
                        var x = new FileInfo(e.FullPath).DirectoryName;
                        Config.ExePath = x;
                        if (fileName == null)
                            return;
                        if (fileName.StartsWith("~"))
                            return;
                        //WorkBookCore.AnalyzerExcel(e.FullPath);
                        new CSVSerialize().AnalyzerExcel(e.FullPath);
                    }
                    catch(Exception exception)
                    {
                        Console.WriteLine(exception);
                        Console.WriteLine($"导出{e.FullPath}失败");
                    }
                }
            }
        }
    }
}