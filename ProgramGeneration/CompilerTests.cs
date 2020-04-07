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
        public int TestFile(string file)
        {
            string outputName = file.Split('/').Last().Split('.').First();
            Compiler c = new Compiler();
            Decompiler decompiler = new Decompiler();
            Processor p = new Processor();

            byte[] prog = null;

            c.AddFile(file);
            c.Compile();

            prog = c.BinaryOutput();
            decompiler.Decompile(prog, String.Format("{0}.dkhl", outputName));
            File.WriteAllBytes(String.Format("{0}.khl", outputName), prog);

            p.LoadProgram(prog);
            return (p.Run());
        }

        public int TestCode(string code)
        {
            Compiler c = new Compiler();
            Processor p = new Processor();

            byte[] prog = null;

            c.AddLines(code.Split());
            c.Compile();

            prog = c.BinaryOutput();

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
        public void BasicTest()
        {
            Assert.AreEqual(42, TestFile("Samples/Test.txt"));
        }
    }
}