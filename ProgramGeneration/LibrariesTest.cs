using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using KhelljyrCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProgramGeneration
{
    [TestClass]
    public class LibrariesTest
    {
        private static readonly string[] Paths = new string[]
        {
            "../../../../STDLib/bin/Debug/netcoreapp2.2/"
        };

        [TestMethod, Priority(5)]
        public static void PrepareLibraries(Processor p, bool runtime)
        {
            List<string> paths = new List<string>();

            foreach (string path in Paths)
            {
                paths.AddRange(Directory.GetFiles(path, "*Lib.dll"));
            }

            p.LibraryHandler.LoadLibraries(paths, runtime);
        }

        [TestMethod]
        public void LibraryStringFormat()
        {
            CompilerTests.TestFile("Samples/LibraryStringFormat.txt", null, true);
        }
    }
}
