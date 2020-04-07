using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCompiler.Containers;
using KhelljyrCompiler.Containers.Instructions;

namespace KhelljyrCompiler
{
    public class Compiler
    {
        public List<string> Builder = new List<string>();
        public List<string> Files = new List<string>();

        public List<Function> Functions = new List<Function>();

        public void AddLines(string[] lines)
        {
            Builder.AddRange(lines);
        }

        public void AddFile(string file)
        {
            Files.Add(file);
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
            ReadFiles();
            Process();
        }

        public void ReadFiles()
        {
            Files.ForEach(i =>
            {
                Builder.AddRange(File.ReadAllLines(i));
            });
        }

        private string TrimLine(string line)
        {
            string a = line.Trim();

            while (a.Contains("  "))
                a = a.Replace("  ", " ");

            return (a);
        }

        public void Process()
        {
            OPInterpretor interpretor = new OPInterpretor();

            Builder.ForEach(i =>
            {
                i = TrimLine(i);

                if (i == "") return;

                interpretor.Treat(this, i.Split(" "));
            });

            DoFunctionPriority();
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

            CreateFunctionTable(functionAddresses).ForEach(final.Add);

            bytes.ForEach(final.Add);
            byte[] program = final.SelectMany(i => i).ToArray();

            ComputeLabelJumps(functionAddresses, program);

            return (program);
        }

        public List<byte[]> CreateFunctionTable(List<int> addresses)
        {
            List<byte[]> functionTable = new List<byte[]>();

            functionTable.Add(BitConverter.GetBytes(addresses.Count));
            addresses.ForEach(i => functionTable.Add(BitConverter.GetBytes(i)));

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

                    byte[] addrToWrite = BitConverter.GetBytes(i.LabelContainer.InstructionPtr);

                    var a = i.InstructionPtr;
                    var b = i.LocalAddressToWrite();
                    var c = count * Defines.SIZE_INT;

                    Array.Copy(addrToWrite, 0, prog,  a + b + c , addrToWrite.Length);
                });

                ++count;
            }

        }
    }
}
