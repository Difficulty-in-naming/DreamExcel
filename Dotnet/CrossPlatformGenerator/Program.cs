using System;
using System.Collections.Generic;
using System.IO;
using DreamExcel.Core;
using Hocon;
using Microsoft.CodeAnalysis;
using NPOI.HSSF.Record;

namespace CrossPlatformGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                if (args[0] == "StartServer")
                {
                    Batch.StartServer(args[1]);
                }
            }
            else
            {
                if (args[1] == "StartServer")
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
            public static void StartServer(string path)
            {
                path = Path.GetDirectoryName(path);
                Console.WriteLine("开启服务器成功" + path);
                _watcher = new FileSystemWatcher(path);
                GC.KeepAlive(_watcher);
                _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName |
                                        NotifyFilters.Attributes | NotifyFilters.Security | NotifyFilters.Size | NotifyFilters.CreationTime;
                _watcher.InternalBufferSize = 1024 * 1024 * 30;
//                fsw.Path = path;
                _watcher.Filter = "*.xlsx";
                _watcher.Changed+=FswOnChanged;
                _watcher.Error += FswOnError;
                _watcher.IncludeSubdirectories = true;

                _watcher.EnableRaisingEvents = true;
                
                List<MetadataReference> references = new List<MetadataReference>();
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
                    if (fileName.StartsWith("~$",StringComparison.OrdinalIgnoreCase))
                        return;
                    if( DateTime.Now.Subtract (_lastTimeFileWatcherEventRaised).TotalMilliseconds < 1000 )
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
                        new ExcelSerialize().AnalyzerExcel(e.FullPath);
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