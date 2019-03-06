using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DreamExcel.Core;
using MicroBatchFramework;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TypeBuilder = System.Reflection.Emit.TypeBuilder;

namespace CrossPlatformGenerator
{
    internal class Program
    {   
        private static async Task Main(string[] args)
        {
            //有Bug需要把路径的反斜杠转换一下
            for (var index = 0; index < args.Length; index++)
            {
                args[index] = args[index].Replace("\\", "/");
            }

            await new HostBuilder()
                .ConfigureLogging(x => x.AddConsole())
                .RunBatchEngine(args);

            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        public class Batch : BatchBase
        {
            public void Gen([Option("p", "导出Excel的路径(填文件名导出单个,填文件夹名称导出该文件夹下所有Excel)")]
                string path)
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

            public void StartServer([Option("p","监听文件夹中的文件变化")]string path)
            {
                path = Path.GetDirectoryName(path);
                FileSystemWatcher fsw = new FileSystemWatcher(path);
                fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Security | NotifyFilters.Size | NotifyFilters.CreationTime;
//                fsw.Path = path;
                fsw.EnableRaisingEvents = true;
                fsw.Changed+=FswOnChanged;
                fsw.IncludeSubdirectories = true;
            }

            private void FswOnChanged(object sender, FileSystemEventArgs e)
            {
                string strFileExt = Path.GetExtension(e.FullPath);
                if (strFileExt == ".xlsx" || strFileExt == ".xls")
                {
                    WorkBookCore.AnalyzerExcel(e.FullPath);
                    Console.WriteLine($"导出{e.FullPath}成功");
                }
            }
        }
    }
}