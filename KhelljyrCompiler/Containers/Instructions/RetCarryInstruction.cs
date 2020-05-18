using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Semantics;

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
            Bytes.Add(OPCodes.Codes.AssignReturnCarry);
            Bytes.Add(Variable.Address);
            Bytes.Add(Variable.Size);

            return (Bytes.Convert());
        }
    }
}
