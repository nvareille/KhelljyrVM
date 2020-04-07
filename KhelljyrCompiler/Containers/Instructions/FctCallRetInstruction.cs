using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;

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
            List<byte[]> bytes = new List<byte[]>();
            RetCarryInstruction i = new RetCarryInstruction(Fct.Variables.First());
            
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

            //DEBUG
            //bytes.Add(i.ByteOutput());


            bytes.Add(GetBytes(OPCodes.Codes.FctPop));


            return (Convert(bytes));
        }
    }
}
