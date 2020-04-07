using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KhelljyrCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProgramGeneration
{
    [TestClass]
    public class UnitTest1
    {
        private byte[] OPCall(OPCodes.Codes c)
        {
            return (BitConverter.GetBytes((int)c));
        }

        private List<byte[]> StackOverflow = new List<byte[]>
        {
            BitConverter.GetBytes(1), // NBR de Fonctions
            BitConverter.GetBytes(0), // Index de la fonction 1

            // Fonction Main
            BitConverter.GetBytes(4),

            //      Prepare Fct
            BitConverter.GetBytes((int)OPCodes.Codes.FctPrepare),
            BitConverter.GetBytes(0),

            //      Start Fct
            BitConverter.GetBytes((int)OPCodes.Codes.FctStart),

            //      Return [Address] [Length]
            BitConverter.GetBytes((int)OPCodes.Codes.Ret),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(4),
        };

        private List<byte[]> Return = new List<byte[]>
        {
            BitConverter.GetBytes(1), // NBR de Fonctions
            BitConverter.GetBytes(0), // Index de la fonction 1

            // Fonction Main
            BitConverter.GetBytes(4),
            
            //      Assign value [Ptr] [size] [value]
            BitConverter.GetBytes((int)OPCodes.Codes.AssignStatic),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(Defines.SIZE_INT),
            BitConverter.GetBytes(42),
            
            //      Return [Address] [Length]
            BitConverter.GetBytes((int)OPCodes.Codes.Ret),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(4),

            //      Function Pop
            BitConverter.GetBytes((int)OPCodes.Codes.FctPop)
        };

        private List<byte[]> Addition = new List<byte[]>
        {
            BitConverter.GetBytes(1), // NBR de Fonctions
            BitConverter.GetBytes(0), // Index de la fonction 1

            // Fonction Main
            BitConverter.GetBytes(8),

            //      Assign value [Ptr] [size] [value]
            BitConverter.GetBytes((int)OPCodes.Codes.AssignStatic),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(Defines.SIZE_INT),
            BitConverter.GetBytes(42),

            //      Assign value [Ptr] [size] [value]
            BitConverter.GetBytes((int)OPCodes.Codes.AssignStatic),
            BitConverter.GetBytes(4),
            BitConverter.GetBytes(Defines.SIZE_INT),
            BitConverter.GetBytes(42),

            //      Addition addr1 addr2 addr3
            BitConverter.GetBytes((int)OPCodes.Codes.OperationAdd),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(4),
            BitConverter.GetBytes(0),

            //      Return [Address] [Length]
            BitConverter.GetBytes((int)OPCodes.Codes.Ret),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(4),

            //      Function Pop
            BitConverter.GetBytes((int)OPCodes.Codes.FctPop),
        };



        private List<byte[]> ReturnMultiple = new List<byte[]>
        {
            BitConverter.GetBytes(2), // NBR de Fonctions
            BitConverter.GetBytes(0), // Index de la fonction 1
            BitConverter.GetBytes(44), // Index de la fonction 2

            // Fonction Main
            BitConverter.GetBytes(4),

            //      Prepare Fct
            BitConverter.GetBytes((int)OPCodes.Codes.FctPrepare),
            BitConverter.GetBytes(1),

            //      Start Fct
            BitConverter.GetBytes((int)OPCodes.Codes.FctStart),

            //      Copy Return Carry
            BitConverter.GetBytes((int)OPCodes.Codes.AssignReturnCarry),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(4),

            //      Return [Address] [Length]
            BitConverter.GetBytes((int)OPCodes.Codes.Ret),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(4),
            
            //      Function Pop
            BitConverter.GetBytes((int)OPCodes.Codes.FctPop),

            // Fonction 2
            BitConverter.GetBytes(4),
            
            //      Assign value [Ptr] [size] [value]
            BitConverter.GetBytes((int)OPCodes.Codes.AssignStatic),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(Defines.SIZE_INT),
            BitConverter.GetBytes(42),

            //      Return [Address] [Length]
            BitConverter.GetBytes((int)OPCodes.Codes.Ret),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(4),

            //      Return [Address] [Length]
            BitConverter.GetBytes((int)OPCodes.Codes.Ret),
            BitConverter.GetBytes(0),
            BitConverter.GetBytes(4),

            //      Function Pop
            BitConverter.GetBytes((int)OPCodes.Codes.FctPop),
        };

        private static void WriteBinary(List<byte[]> prg, string output)
        {
            byte[] str = prg.SelectMany(i => i).ToArray();

            File.WriteAllBytes(output, str);
        }

        [TestMethod, Priority(5)]
        public void GenerateBinary()
        {
            WriteBinary(StackOverflow, "out1.khl");
            WriteBinary(Return, "out2.khl");
            WriteBinary(ReturnMultiple, "out3.khl");
            WriteBinary(Addition, "out4.khl");
        }

        //[TestMethod]
        public void LoadProgramOverflow()
        {
            byte[] prg = File.ReadAllBytes("out1.khl");
            Processor proc = new Processor();

            proc.LoadProgram(prg);
            proc.Run();
        }

        [TestMethod]
        public void LoadProgramReturn()
        {
            byte[] prg = File.ReadAllBytes("out2.khl");
            Processor proc = new Processor();

            proc.LoadProgram(prg);
            Assert.AreEqual(proc.Run(), 42);
        }

        [TestMethod]
        public void LoadProgramReturnMultiple()
        {
            byte[] prg = File.ReadAllBytes("out3.khl");
            Processor proc = new Processor();

            proc.LoadProgram(prg);
            Assert.AreEqual(proc.Run(), 42);
        }

        [TestMethod]
        public void LoadProgramAddition()
        {
            byte[] prg = File.ReadAllBytes("out4.khl");
            Processor proc = new Processor();

            proc.LoadProgram(prg);
            Assert.AreEqual(proc.Run(), 84);
        }
    }
}
