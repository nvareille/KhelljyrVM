using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using KhelljyrCommon;
using KhelljyrCommon.Libraries;
using KhelljyrCommon.Semantics;

namespace KhelljyrCompiler
{
    public class Compiler
    {
        public Preprocessor Preprocessor = new Preprocessor();
        public List<Template> Templates = new List<Template>();
        public List<GeneratedTemplate> GeneratedTemplates = new List<GeneratedTemplate>();
        public OPInterpretor OPInterpretor = new OPInterpretor();
        public LibraryHandler LibraryHandler = new LibraryHandler();
        public List<KhelljyrCommon.Tuple<string, int, string>> Builder = new List<KhelljyrCommon.Tuple<string, int, string>>();
        public List<string> Files = new List<string>();
        public object ProcessingBlock;
        public List<Function> Functions = new List<Function>();
        public List<Structure> Structures = new List<Structure>();
        public StaticSymbolLibrary Symbols = new StaticSymbolLibrary();
        
        public void AddFile(string file)
        {
            Files.Add(file);
        }

        public void AddPreprocessorFile(string file)
        {
            Preprocessor.AddFile(file);
        }

        public void AddFiles(string[] files)
        {
            foreach (string file in files)
            {
                AddFile(file);
            }
        }

        public void Compile()
        {
            Preprocessor.ComputePreprocessing();
            ReadFiles();
            Preprocess();
            GenTemplates();
            Process();
        }

        private void GenTemplates()
        {
            bool templated = false;
            Template t = null;
            StringBuilder current = new StringBuilder();
            List<Tuple<string, int>> toRemove = new List<Tuple<string, int>>();
            
            Builder.ForEach(i =>
            {
                if (i.Item3.StartsWith("#TEMPLATE "))
                {
                    string line = i.Item3.Replace("#TEMPLATE ", "");
                    string[] args = line.Split(" ");

                    t = new Template
                    {
                        Name = args[0],
                        Tokens = args.Skip(1).ToList()
                    };
                    templated = true;

                    toRemove.Add(new Tuple<string, int>(i.Item1, i.Item2));
                }
                else if (templated && !i.Item3.StartsWith("#END"))
                {
                    current.Append(i.Item3 + "\n");
                    toRemove.Add(new Tuple<string, int>(i.Item1, i.Item2));
                }

                if (i.Item3.StartsWith("#END"))
                {
                    toRemove.Add(new Tuple<string, int>(i.Item1, i.Item2));
                    t.Content = current.ToString();
                    Templates.Add(t);
                    current.Clear();
                    templated = false;
                }
            });

            toRemove.ForEach(i =>
            {
                Builder.RemoveAll(o => (o.Item1 == i.Item1 && o.Item2 == i.Item2));
            });

            ReplaceTemplateCode();
        }

        private void ReplaceTemplateCode()
        {
            Templates.ForEach(t =>
            {
                Builder.ForEach(i =>
                {
                    var found = Regex.Matches(i.Item3, t.Name + "<(.*)>");
                    
                    foreach (Match o in found)
                    {
                        string str = o.Groups[1].Value;
                        GeneratedTemplate temp = t.Generate(str.Replace(" ", "").Split(","));

                        GeneratedTemplates.Add(temp);
                        i.Item3 = i.Item3.Replace(t.Name + "<" + str + ">", temp.Name);
                    }
                });
            });

            GeneratedTemplates.ForEach(i =>
            {
                List<KhelljyrCommon.Tuple<string, int, string>> a = new List<KhelljyrCommon.Tuple<string, int, string>>();

                foreach (string s in i.Content.Split("\n"))
                {
                    a.Add(new KhelljyrCommon.Tuple<string, int, string>("Templated Function " + i.Name, 0, s));
                }

                a.Reverse();
                a.ForEach(o => Builder.Insert(0, o));
            });

            
        }

        public void ReadFiles()
        {
            Files.ForEach(i =>
            {
                int count = 0;
                
                Builder.AddRange(File.ReadAllLines(i).Select(o => new KhelljyrCommon.Tuple<string, int, string>(i, ++count, o)));
            });
        }

        private string TrimLine(string line)
        {
            string a = line.Trim();

            while (a.Contains("  "))
                a = a.Replace("  ", " ");

            return (a);
        }

        public void Preprocess()
        {
            List<KhelljyrCommon.Tuple<string, int, string>> preprocessed = new List<KhelljyrCommon.Tuple<string, int, string>>();

            Builder.ForEach(i =>
            {
                string str = i.Item3;
                string[] a = str.Split(" ");

                foreach (KeyValuePair<string, string> pair in Preprocessor.Values)
                {
                    int count = 0;

                    while (count < a.Length)
                    {
                        if (pair.Key == a[count])
                            a[count] = pair.Value;
                        ++count;
                    }
                }

                str = string.Join(" ", a);

                preprocessed.Add(new KhelljyrCommon.Tuple<string, int, string>(i.Item1, i.Item2, str));
            });

            Builder = preprocessed;
        }

        public void Process()
        {
            KhelljyrCommon.Tuple<string, int, string> line = null;
            
            try
            {
                Builder.ForEach(i =>
                {
                    line = i;
                    Argument[] strs = LineParser.GetArgs(line.Item3);

                    if (strs.Length != 0)
                        OPInterpretor.Treat(this, strs);
                });

                DoFunctionPriority();
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Compilation Error: [{0}, {1}] {2}", line.Item1, line.Item2, line.Item3));
            }
        }

        private void DoFunctionPriority()
        {
            int id = 0;

            Functions.ForEach(i =>
            {
                if (i.Name == "main")
                    i.Prio = 100;
            });

            Functions = Functions.OrderByDescending(i => i.Prio).ToList();
            Functions.ForEach(i => i.Id = id++);
        }

        public byte[] BinaryOutput()
        {
            int addr = 0;
            int localAddr = 0;

            List<int> functionAddresses = new List<int>();
            List<byte[]> bytes = new List<byte[]>();

            Functions.ForEach(i =>
            {
                byte[] b = null;
                List<byte[]> fctBytes = new List<byte[]>();

                localAddr += Defines.SIZE_INT;

                fctBytes.Add(BitConverter.GetBytes(i.Variables.Sum(o => o.Size)));

                i.Instructions.ForEach(o =>
                {
                    o.Function = i;

                    byte[] output = o.ByteOutputInternal(localAddr);

                    localAddr += output.Length;
                    fctBytes.Add(output);
                });

                b = fctBytes.SelectMany(o => o).ToArray();
                bytes.Add(b);

                functionAddresses.Add(addr);
                addr += b.Length;
            });
            
            List<byte[]> final = new List<byte[]>();

            byte[] symbols = CreateSymbolsTable();
            CreateFunctionTable(functionAddresses).ForEach(final.Add);

            final.Add(symbols);
            bytes.ForEach(final.Add);
            byte[] program = final.SelectMany(i => i).ToArray();
            
            ComputeLabelJumps(functionAddresses, program);

            return (program);
        }

        public byte[] CreateSymbolsTable()
        {
            int size = Symbols.TableSize;
            byte[] symbols = new byte[size];

            foreach (KeyValuePair<string, uint> pair in Symbols.Symbols)
            {
                Array.Copy(pair.Key.Select(i => (byte)i).ToArray(), 0, symbols, pair.Value, pair.Key.Length);
            }

            return (symbols);
        }

        public List<byte[]> CreateFunctionTable(List<int> addresses)
        {
            List<byte[]> functionTable = new List<byte[]>();

            functionTable.Add(BitConverter.GetBytes(addresses.Count));
            addresses.ForEach(i => functionTable.Add(BitConverter.GetBytes(i + Symbols.TableSize)));

            return (functionTable);
        }

        public void ComputeLabelJumps(List<int> addresses, byte[] prog)
        {
            int count = 1;

            foreach (Function function in Functions)
            {
                function.Instructions.OfType<Jumpable>().ToList().ForEach(i =>
                {
                    if (i.Label == null)
                        return;

                    i.ComputeLabelPosition(function);

                    byte[] addrToWrite = BitConverter.GetBytes(i.LabelContainer.InstructionPtr + Symbols.TableSize);

                    var a = i.InstructionPtr;
                    var b = i.LocalAddressToWrite();
                    var c = Symbols.TableSize;
                    var d = (Functions.Count + 1) * Defines.SIZE_INT;
                    int e = a + b + c + d;

                    Array.Copy(addrToWrite, 0, prog,  a + b + c + d, addrToWrite.Length);
                });

                ++count;
            }

        }
    }
}
