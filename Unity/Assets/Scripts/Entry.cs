using System;
using System.IO;
using Windows;
using DreamExcel.Core;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;

public class Entry : MonoBehaviour
{
    private static DateTime _lastTimeFileWatcherEventRaised;
    private static FileSystemWatcher _watcher;
    public string MonitorDir;
    private void Start()
    {
        string path;
#if !UNITY_EDITOR
        string env = Environment.CommandLine;
        var split = env.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        path = split[3];

        ConsoleWindow window = new ConsoleWindow();
        window.Initialize();
#else
        UnitySystemConsoleRedirector.Redirect();
        path = MonitorDir;
#endif
        Console.WriteLine(path);
        StartServer(path);
    }

    public static void StartServer(string path)
    {
        Console.WriteLine("开启服务器成功");
        path = Path.GetDirectoryName(path);
        _watcher = new FileSystemWatcher(path);
        GC.KeepAlive(_watcher);
        _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size |
                                NotifyFilters.CreationTime;
        _watcher.InternalBufferSize = 1024 * 1024 * 30;
//                fsw.Path = path;
        _watcher.Filter = "*.xlsx";
        _watcher.Changed += FswOnChanged;
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
            Console.WriteLine(new FileInfo(e.Name + "    " + e.FullPath));
            string fileName = Path.GetFileName(e.FullPath);
            if (fileName.StartsWith("~$", StringComparison.OrdinalIgnoreCase))
                return;
            if (DateTime.Now.Subtract(_lastTimeFileWatcherEventRaised).TotalMilliseconds < 1000)
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
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.WriteLine($"导出{e.FullPath}失败");
            }
        }
    }
}