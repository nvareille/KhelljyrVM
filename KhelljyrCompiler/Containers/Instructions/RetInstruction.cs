using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Semantics;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class RetInstruction : Instruction
    {
        public override byte[] ByteOutput()
        {
            Bytes.Add(OPCodes.Codes.FctPop);

            return (Bytes.Convert());
        }
    }
}
