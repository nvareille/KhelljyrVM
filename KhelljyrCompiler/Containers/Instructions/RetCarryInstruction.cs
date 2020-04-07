using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class RetCarryInstruction : Instruction
    {
        private Variable Variable;

        public RetCarryInstruction(Variable v)
        {
            Variable = v;
        }

        public override byte[] ByteOutput()
        {
            List<byte[]> bytes = new List<byte[]>();

            bytes.Add(GetBytes(OPCodes.Codes.AssignReturnCarry));
            bytes.Add(GetBytes(Variable.Address));
            bytes.Add(GetBytes(Variable.Size));

            return (Convert(bytes));
        }
    }
}
