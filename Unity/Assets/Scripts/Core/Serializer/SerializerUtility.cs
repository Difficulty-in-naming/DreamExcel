using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CrossPlatformGenerator.Core.Utility;
using DreamExcel.Core;
using Hocon;
using Hocon.Json;
using MemoryPack;
using MemoryPack.Compression;
using MemoryPack.Generator;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NPOI.SS.Util;

namespace CrossPlatformGenerator.Core
{
    public static class SerializerUtility
    {
        private static Dictionary<string, Assembly> Assemblies = new();
        
        public static void WriteJson(this ExcelSerialize.CSVRecords records,string dbFilePath)
        {
            JsonSerializerSettings currentSettings = JsonConvert.DefaultSettings?.Invoke() ?? new JsonSerializerSettings();
            currentSettings.Converters.Add(new StringEnumConverter());
            JsonConvert.DefaultSettings = () => currentSettings;
            //var item = new Dictionary<object, object>();
            var jObject = GetJObject(records);

            File.WriteAllText(dbFilePath + ".json", jObject.ToString());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("导出Json文件| " + new DirectoryInfo(dbFilePath).FullName + ".json");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static JObject GetJObject(ExcelSerialize.CSVRecords records)
        {
            var jObject = new JObject();
            for (var j = Config.StartLine; j <= records.RowCount; j++)
            {
                JObject jValue = new JObject();
                var valueRow = records.Rows[j];
                if (valueRow == null)
                    continue;
                try
                {
                    var key = valueRow[records.GetIdIndex()];

                    var column = valueRow;
                    for (var i = 0; i < column.Length; i++)
                    {
                        try
                        {
                            var name = records.GetName(i);
                            var type = records.GetType(i);
                            if (string.IsNullOrEmpty(name) || name.StartsWith("*",StringComparison.OrdinalIgnoreCase))
                                continue;
                            var valueCell = column[i];
                            if (valueCell != null && !string.IsNullOrEmpty(valueCell))
                            {
                                if (type is "int" or "long" or "short" or "uint" or "ushort" or "byte")
                                    jValue.Add(name, long.Parse(valueCell));
                                else if (type == "bool")
                                    jValue.Add(name, Parse.BoolParse(valueCell));
                                else if (type is "bool[]" or "List<bool>")
                                    jValue.Add(name, new JArray(valueCell.Split(Config.SplitChar1, StringSplitOptions.RemoveEmptyEntries).Select(Parse.BoolParse).ToArray()));
                                else if (type == "ulong")
                                    jValue.Add(name, ulong.Parse(valueCell));
                                else if (type is "float" or "double")
                                    jValue.Add(name, double.Parse(valueCell));
                                else if (type == "string")
                                    jValue.Add(name, valueCell);
                                else if (type is "int[]" or "long[]" or "short[]" or "uint[]" or "ushort[]" or "byte[]" or "List<long>" or "List<short>" or "List<uint>" or "List<ushort>" or "List<byte>")
                                    jValue.Add(name, new JArray(valueCell.Split(Config.SplitChar1, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()));
                                else if (type is "ulong[]" or "List<ulong>")
                                    jValue.Add(name, new JArray(valueCell.Split(Config.SplitChar1, StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray()));
                                else if (type is "float[]" or "double[]" or "List<float>" or "List<double>")
                                    jValue.Add(name, new JArray(valueCell.Split(Config.SplitChar1, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray()));
                                else if (type is "string[]" or "List<string>" or "HashSet<string>" or "IEnumerable<string>")
                                    jValue.Add(name, new JArray(valueCell.Split(Config.SplitChar1, StringSplitOptions.RemoveEmptyEntries)));
                                else if(type is "DateTime" or "DateTimeOffset")
                                    jValue.Add(name, valueCell);
                                else
                                {
                                    try
                                    {
                                        var value = valueCell;
                                        var parse = HoconParser.Parse(value);
                                        var token = parse.ToJToken();
                                        jValue.Add(name, token);
                                    }
                                    catch
                                    {
                                        jValue.Add(name, valueCell);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            var cell = new CellReference(j, i);
                            throw new ExcelException($"单元格:{cell.FormatAsString()} 格式错误 \n " + e);
                        }
                    }
                    jObject.Add(key, jValue);
                }
                catch(Exception e)
                {
                    var cell = new CellReference(j, records.GetIdIndex());
                    throw new ExcelException($"单元格:{cell.FormatAsString()} 格式错误,这个单元格可能包含了空字符的内容导致写入失败.尝试删除行后重新保存试试\n" + e);
                }
            }
            return jObject;
        }
        
        public static void WriteBytes(this ExcelSerialize.CSVRecords records, string dbFilePath, string script, string fileName,BytesFlag bytesFlag)
        {
            var getJObject = GetJObject(records);
            if (!Assemblies.TryGetValue(script, out var assembly))
            {
                List<SyntaxTree> classes = new List<SyntaxTree>();
                //script = script.Replace(" class ", " class ___");
                classes.Add(CSharpSyntaxTree.ParseText(script));
                List<MetadataReference> references = new List<MetadataReference>();
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var node in assemblies)
                {
                    try
                    {
                        references.Add(MetadataReference.CreateFromFile(node.Location));
                    }
                    catch(Exception e)
                    {
                        //Console.Write(e);
                    }
                }
                for (var index = 0; index < Config.Info.ReferenceDlls.Length; index++)
                {
                    var dir = Config.Info.ReferenceDlls[index];
                    dir = Config.ExePath + Path.DirectorySeparatorChar + dir;
                    foreach (var node in Directory.GetFiles(dir))
                    {
                        if (node.EndsWith(".dll"))
                        {
                            try
                            {
                                var bytes = File.ReadAllBytes(node);
                                Assembly.Load(bytes);
                                if(!node.Contains("MemoryPack",StringComparison.OrdinalIgnoreCase) && !node.Contains("MessagePack",StringComparison.OrdinalIgnoreCase))
                                    references.Add(MetadataReference.CreateFromFile(node));
                            }
                            catch(Exception e)
                            {
                                //Console.WriteLine(e);
                                //Ignore
                            }
                        }
                    }
                }

                var op = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
                var compilation = CSharpCompilation.Create(fileName + Guid.NewGuid(), classes, references, op);
                //用于MemoryPack生成代码
                if (bytesFlag == BytesFlag.MemoryPack)
                {
                    CSharpGeneratorDriver x = CSharpGeneratorDriver.Create(new List<ISourceGenerator> { new MemoryPackGenerator()});
                    x.RunGeneratorsAndUpdateCompilation(compilation, out var newCompilation, out var diagnostic);
                    foreach (var node in diagnostic)
                    {
                        Console.WriteLine(node.GetMessage());
                    }
                    compilation = (CSharpCompilation)newCompilation;
                }

                //Emit to stream
                var ms = new MemoryStream();
                var emitResult = compilation.Emit(ms);
                foreach (var node in emitResult.Diagnostics)
                {
                    if (node.DefaultSeverity == DiagnosticSeverity.Error)
                    {
                        Console.WriteLine(node.GetMessage());
                    }
                }

                assembly = Assembly.Load(ms.ToArray());
                //Assemblies.Add(script, assembly);
            }
            try
            {
                var type = assembly.GetTypeByName(fileName);
                var t = typeof(Dictionary<,>).MakeGenericType(records.GetIdType() == "int" ? typeof(int) : typeof(string), type);
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
                jsonSerializerSettings.FloatParseHandling = FloatParseHandling.Decimal;
                object g = JsonConvert.DeserializeObject(getJObject.ToString(), t, jsonSerializerSettings);
                if (bytesFlag == BytesFlag.MessagePack)
                {
                    var options = MessagePackSerializerOptions.Standard.WithResolver(StandardResolver.Instance).WithCompression(MessagePackCompression.Lz4Block);
                    using var fileStream = new FileStream(dbFilePath + ".bytes",FileMode.OpenOrCreate,FileAccess.ReadWrite);
                    MessagePackSerializer.Serialize(t, fileStream,g, options);
                }
                else if (bytesFlag == BytesFlag.MemoryPack)
                {
                    var registerFormatterMethod = type.GetMethod("RegisterFormatter", BindingFlags.Public | BindingFlags.Static);
                    registerFormatterMethod.Invoke(null, null);
                    using var compressor = new BrotliCompressor();
                    var bytes = MemoryPackSerializer.Serialize(t,g);
                    compressor.Write(bytes);
                    var b = compressor.ToArray();
                    File.WriteAllBytes(dbFilePath + ".bytes", b);
                }
                else
                {
                    using var fileStream = new FileStream(dbFilePath + ".bytes",FileMode.OpenOrCreate,FileAccess.ReadWrite);
                    ProtoBuf.Serializer.Serialize(fileStream, g);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"导出{bytesFlag.ToString()}文件| " + new DirectoryInfo(dbFilePath).FullName + ".bytes");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {                
                        if(!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                Console.WriteLine(errorMessage);
            }
        }
    }
}