﻿using KhelljyrCompiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrDecompiler;

namespace ProgramGeneration
{
    [TestClass]
    public class CompilerTests
    {
        public static int TestFile(string file, IEnumerable<string> preprocessor = null, bool onlyCompile = false)
        {
            File.Copy("../../../../Docs/" + file, file, true);
            
            string outputName = file.Split('/').Last().Split('.').First();
            Processor p = new Processor();
            Compiler c = new Compiler();
            Decompiler decompiler = new Decompiler();

            LibrariesTest.PrepareLibraries(p, false);
            c.LibraryHandler = p.LibraryHandler;
            byte[] prog = null;

            if (preprocessor != null)
            {
                foreach (string s in preprocessor)
                {
                    c.AddPreprocessorFile(s);
                }
            }

            c.AddFile(file);
            c.Compile();

            prog = c.BinaryOutput();
            File.WriteAllBytes(String.Format("{0}.khl", outputName), prog);
            decompiler.Decompile(prog, String.Format("{0}.dkhl", outputName));
            
            if (onlyCompile)
                return (0);

            p = new Processor();
            LibrariesTest.PrepareLibraries(p, true);
            p.LoadProgram(prog);
            return (p.Run());
        }
        
        [TestMethod]
        public void BasicAdd()
        {
           Assert.AreEqual(84, TestFile("Samples/BasicAdd.txt"));
        }

        [TestMethod]
        public void AdvancedAdd()
        {
            Assert.AreEqual(126, TestFile("Samples/AdvancedAdd.txt"));
        }

        [TestMethod]
        public void BasicFct()
        {
            Assert.AreEqual(84, TestFile("Samples/BasicFct.txt"));
        }

        [TestMethod]
        public void BasicFctVar()
        {
            Assert.AreEqual(126, TestFile("Samples/BasicFctVar.txt"));
        }

        [TestMethod]
        public void BasicLoop()
        {
            Assert.AreEqual(10, TestFile("Samples/BasicLoop.txt"));
        }

        [TestMethod]
        public void BasicPreprocessing()
        {
            Assert.AreEqual(42, TestFile("Samples/BasicPreprocessing.txt", new List<string>
            {
                "Samples/Defines.txt"
            }));
        }

        [TestMethod]
        public void BasicCast()
        {
            Assert.AreEqual(43, TestFile("Samples/BasicCast.txt"));
        }

        [TestMethod]
        public void BasicPtr()
        {
            Assert.AreEqual(42, TestFile("Samples/BasicPtr.txt"));
        }

        [TestMethod]
        public void AddFromPtr()
        {
            Assert.AreEqual(42, TestFile("Samples/AddFromPtr.txt"));
        }

        [TestMethod]
        public void AdvancedPtr()
        {
            Assert.AreEqual(42, TestFile("Samples/AdvancedPtr.txt"));
        }

        [TestMethod]
        public void BasicLibCall()
        {
            Assert.AreEqual(126, TestFile("Samples/BasicLibCall.txt"));
        }

        [TestMethod]
        public void BasicStructures()
        {
            Assert.AreEqual(84, TestFile("Samples/BasicStructures.txt"));
        }

        [TestMethod]
        public void BasicTest()
        {
            Assert.AreEqual(42, TestFile("Samples/Test.txt"));
        }
    }
}
