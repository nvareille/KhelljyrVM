using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrDecompiler
{
    public class Outputer
    {
        public StringBuilder Builder = new StringBuilder();

        public string Print(string str, params object[] objs)
        {
            return (string.Format(str, objs));
        }

        public string Merge(List<string> strs)
        {
            return (string.Join('\n', strs));
        }

        public string FunctionTable(IEnumerable<int> idxs)
        {
            List<string> strs = new List<string>();
            int count = 1;

            strs.Add(Print("{0} Functions:", idxs.Count()));

            foreach (int i in idxs)
            {
                strs.Add(Print("Function {0}: {1}", count, i));
                ++count;
            }

            return (BuildMerge(strs));
        }

        public string OutputFunctionStart(int idx, int id, int size)
        {
            List<string> strs = new List<string>();

            strs.Add(Print("\nFunction {0} {1} Memory Bytes\n[{2}] Stack allocation", id, size == 0 ? 4 : size, idx));

            return (BuildMerge(strs));
        }

        public void WriteFile(string path)
        {
            File.WriteAllText(path, Builder.ToString());
        }

        public string BuildMerge(List<string> strs)
        {
            string str = Merge(strs);

            Builder.Append(str);
            Builder.Append("\n");

            return (str);
        }

        public string OutputInstruction(int idx, OPCodes.Codes code, IEnumerable<string> args)
        {
            List<string> strs = new List<string>();

            strs.Add(Print("[{0}] {1} {2}", idx, code.ToString(), String.Join(" ", args)));

            return (BuildMerge(strs));
        }
    }
}
