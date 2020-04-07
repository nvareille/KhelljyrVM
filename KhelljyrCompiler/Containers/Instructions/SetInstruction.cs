using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class SetInstruction : Instruction
    {
        public Variable Variable;
        public int Value;

        public override byte[] ByteOutput()
        {
            List<byte[]> bytes = new List<byte[]>();

            bytes.Add(GetBytes(OPCodes.Codes.AssignStatic));
            bytes.Add(GetBytes(Variable.Address));
            bytes.Add(GetBytes(Defines.SIZE_INT));
            bytes.Add(GetBytes(Value));

            return (Convert(bytes));
        }
    }
}
