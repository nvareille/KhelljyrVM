using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class FctCallInstruction : Instruction
    {
        private Function Fct;
        public List<Variable> Variables = new List<Variable>();

        public FctCallInstruction(Function fct)
        {
            Fct = fct;
        }

        public override byte[] ByteOutput()
        {
            int addr = 0;
            List<byte[]> bytes = new List<byte[]>();

            bytes.Add(GetBytes(OPCodes.Codes.FctPrepare));
            bytes.Add(GetBytes(Fct.Id));

            // arguments à gérer
            foreach (Variable variable in Variables)
            {
                if (variable is IConstVariable)
                {
                    bytes.Add(GetBytes(OPCodes.Codes.VarConstCopy));
                    bytes.Add(GetBytes(variable.Size));
                    bytes.Add(variable.As<IConstVariable>().GetValueAsBytes());
                    bytes.Add(GetBytes(addr));
                }
                else
                {
                    bytes.Add(GetBytes(OPCodes.Codes.VarFctCopy));
                    bytes.Add(GetBytes(variable.Size));
                    bytes.Add(GetBytes(variable.Address));
                    bytes.Add(GetBytes(addr));
                }

                addr += variable.Size;
            }

            bytes.Add(GetBytes(OPCodes.Codes.FctStart));
            
            return (Convert(bytes));
        }
    }
}
