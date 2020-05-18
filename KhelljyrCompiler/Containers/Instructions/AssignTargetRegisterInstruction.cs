using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Semantics;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class AssignTargetRegisterInstruction : Instruction
    {
        private int Index;
        private Variable Variable;

        public AssignTargetRegisterInstruction(Variable v, int index)
        {
            Index = index;
            Variable = v;
        }

        public override byte[] ByteOutput()
        {
            Bytes.Add(OPCodes.Codes.AssignTargetRegister);
            Bytes.Add(Index);
            Bytes.Add(Variable.Target);

            return (Bytes.Convert());
        }
    }
}
