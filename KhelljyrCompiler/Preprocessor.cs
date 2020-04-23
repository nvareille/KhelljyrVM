using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KhelljyrCompiler
{
    public class Preprocessor
    {
        public Dictionary<string, string> Values = new Dictionary<string, string>();
        public List<string> PreprocessorLines = new List<string>();

        public void ComputePreprocessing()
        {
            PreprocessorLines.ForEach(DigestLine);
        }

        public void DigestLine(string str)
        {
            string[] args = str.Split(' ');

            if (args.Length == 0)
                return;

            if (args[0] == "#DEFINE")
                Values.Add(args[1], args[2]);
        }

        public void AddFile(string file)
        {
            PreprocessorLines.AddRange(File.ReadAllLines(file));
        }

        public void AddLine(string line)
        {
            PreprocessorLines.Add(line);
        }
    }
}
