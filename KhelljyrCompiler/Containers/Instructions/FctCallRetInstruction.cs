using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Semantics;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class FctCallRetInstruction : Instruction
    {
        private Function Fct;
        public List<Variable> Variables = new List<Variable>();

        public FctCallRetInstruction(Function fct)
        {
            Fct = fct;
        }

        public override byte[] ByteOutput()
        {
            int addr = 0;
            RetCarryInstruction i = new RetCarryInstruction(Fct.Variables.First());
            
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
            Bytes.Add(OPCodes.Codes.FctPop);
            
            return (Bytes.Convert());
        }
    }
}
