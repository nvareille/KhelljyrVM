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
    }
}
