using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Libraries;
using KhelljyrCommon.Semantics;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class LibraryFctCallInstruction : FctCallInstruction
    {
        private string Lib;
        private string Fct;

        public LibraryFctCallInstruction(string lib, string fct)
        {
            Lib = lib;
            Fct = fct;
        }

        public override byte[] ByteOutput()
        {
            int addr = 0;

            Bytes.Add(OPCodes.Codes.LibCall);
            Bytes.Add(Variables.Sum(i => i.Size));
            Bytes.Add(Lib.Length);
            Bytes.Add(Fct.Length);
            Bytes.Add(Lib);
            Bytes.Add(Fct);

            // arguments à gérer
            foreach (Variable variable in Variables)
            {
                if (variable is IConstVariable)
                {
                    Bytes.Add(OPCodes.Codes.VarConstCopy);
                    Bytes.Add(variable.Size);
                    Bytes.Add(variable.As<IConstVariable>().GetValueAsBytes());
                    Bytes.Add(addr);
                }
                else
                {
                    Bytes.Add(OPCodes.Codes.VarFctCopy);
                    Bytes.Add(variable.Size);
                    Bytes.Add(variable.Address);
                    Bytes.Add(addr);
                }

                addr += variable.Size;
            }

            Bytes.Add(OPCodes.Codes.FctStart);

            return (Bytes.Convert());
        }
    }

    public class FctCallInstruction : Instruction
    {
        private Function Fct;
        public List<Variable> Variables = new List<Variable>();

        protected FctCallInstruction() { }

        public FctCallInstruction(Function fct)
        {
            Fct = fct;
        }

        public override byte[] ByteOutput()
        {
            int addr = 0;

            Bytes.Add(OPCodes.Codes.FctPrepare);
            Bytes.Add(Fct.Id);

            // arguments à gérer
            foreach (Variable variable in Variables)
            {
                if (variable is IConstVariable)
                {
                    Bytes.Add(OPCodes.Codes.VarConstCopy);
                    Bytes.Add(variable.Size);
                    Bytes.Add(variable.As<IConstVariable>().GetValueAsBytes());
                    Bytes.Add(addr);
                }
                else
                {
                    Bytes.Add(OPCodes.Codes.VarFctCopy);
                    Bytes.Add(variable.Size);
                    Bytes.Add(variable.Address);
                    Bytes.Add(addr);
                }

                addr += variable.Size;
            }

            Bytes.Add(OPCodes.Codes.FctStart);
            
            return (Bytes.Convert());
        }

        public static FctCallInstruction GetCallInstruction(Compiler cmp, string[] args, ref int startIdx)
        {
            int idx = startIdx;
            Function fctToCall = cmp.Functions.FirstOrDefault(i => i.Name == args[idx]);
            FctCallInstruction c = null;

            if (fctToCall == null)
            {
                InvocableLibrary l = cmp.LibraryHandler.LoadedLibraries.First(i => i.Library.Name == args[idx]);
                if (l.AvailableMethods.Contains(args[startIdx + 1]))
                {
                    c = new LibraryFctCallInstruction(args[startIdx], args[startIdx + 1]);
                    ++startIdx;
                }
            }
            else
                c = new FctCallInstruction(fctToCall);
            
            return (c);
        }

        public void ExtractVariables(Function fct, string[] args, int count)
        {
            int value = 0;

            while (args.Length > count)
            {
                Variable v = fct.GetVariable(args[count]);

                if (v == null && Int32.TryParse(args[count], out value))
                {
                    v = new ConstIntVariable(value);
                }

                Variables.Add(v);
                ++count;
            }
        }
    }
}
