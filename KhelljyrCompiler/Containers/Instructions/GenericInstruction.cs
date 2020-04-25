using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class GenericInstruction : Instruction
    {
        private OPCodes.Codes Code;

        public GenericInstruction(OPCodes.Codes code)
        {
            Code = code;
        }

        public override byte[] ByteOutput()
        {
            Bytes.Add(Code);

            return (Bytes.Convert());
        }
    }
}
