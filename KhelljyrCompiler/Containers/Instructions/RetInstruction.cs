using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

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
