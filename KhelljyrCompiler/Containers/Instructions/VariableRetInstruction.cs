using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class VariableRetInstruction : Instruction
    {
        public Variable Variable;

        public VariableRetInstruction(Variable v)
        {
            Variable = v;
        }

        public override byte[] ByteOutput()
        {
            List<byte[]> bytes = new List<byte[]>();

            if (Variable is IConstVariable)
            {
                bytes.Add(GetBytes(OPCodes.Codes.SetReturnCarry));
                bytes.Add(GetBytes(Variable.Size));
                bytes.Add(Variable.As<IConstVariable>().GetValueAsBytes());
            }
            else
            {
                bytes.Add(GetBytes(OPCodes.Codes.Ret));
                bytes.Add(GetBytes(Variable.Address));
                bytes.Add(GetBytes(Defines.SIZE_INT));
            }

            bytes.Add(GetBytes(OPCodes.Codes.FctPop));

            return (Convert(bytes));
        }
    }
}
