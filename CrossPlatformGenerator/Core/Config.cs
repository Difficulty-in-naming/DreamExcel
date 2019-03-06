using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DreamExcel.Core
{
    public class Config
    {
        private string mSaveDbPath;

        private string mSaveScriptPath;
        
        public static Config Instance
        {
            get
            {
                var mInstance = new Config();
                //如果工作目录下存在配置文件则读取工作目录下的配置
                var configPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/Config.txt";
                string content;
                if (File.Exists(configPath))
                    content = File.ReadAllText(configPath);
                else
                    content = File.ReadAllText(CurrentPath + "/Config.txt");
                content = Regex.Replace(content, @"\/\*((?:[^*]|(?:\*(?=[^\/])))*)\*\/", "");
                content = content.Replace("\n", "").Replace("\r", "");
                var split = content.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < split.Length; i++)
                    if (split[i].StartsWith(nameof(SaveScriptPath)))
                        mInstance.SaveScriptPath = GetValue(split[i]);
                    else if (split[i].StartsWith(nameof(SaveDbPath)))
                        mInstance.SaveDbPath = GetValue(split[i]);
                    else if (split[i].StartsWith(nameof(ScriptNameSpace)))
                        mInstance.ScriptNameSpace = GetValue(split[i]);
                    else if (split[i].StartsWith(nameof(FileSuffix)))
                        mInstance.FileSuffix = GetValue(split[i]);
                    else if (split[i].StartsWith(nameof(EnumSuffix)))
                        mInstance.EnumSuffix = GetValue(split[i]);
                    else if (split[i].StartsWith(nameof(GeneratorType)))
                        mInstance.GeneratorType = GetValue(split[i]);

                return mInstance;
            }
        }

        private static string CurrentPath => AppDomain.CurrentDomain.BaseDirectory;

        public string ScriptTemplatePath
        {
            get
            {
                var configPath = System.Reflection.Assembly.GetExecutingAssembly().Location + "/GenerateTemplate.txt";
                if (File.Exists(configPath))
                    return configPath;
                return CurrentPath + "/GenerateTemplate.txt";
            }
        }

        public string SaveScriptPath
        {
            get
            {
                if (mSaveScriptPath.Contains(":")) //盘符标志
                    return mSaveScriptPath;
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + mSaveScriptPath;
            }
            private set { mSaveScriptPath = value; }
        }
        public string GeneratorType { get; private set; } 
        public string ScriptNameSpace { get; private set; }
        public string FileSuffix { get; private set; }
        public string EnumSuffix { get; private set; }
        public string SaveDbPath
        {
            get
            {
                if (mSaveDbPath.Contains(":")) //盘符标志
                    return mSaveDbPath;
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + mSaveDbPath;
            }
            set { mSaveDbPath = value; }
        }

        private static string GetValue(string split)
        {
            return split.Substring(split.IndexOf("=") + 1).Trim();
        }
    }
}