using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class CastInstruction : Instruction
    {
        private Variable V1;
        private Variable V2;

        public CastInstruction(Variable v1, Variable v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public override byte[] ByteOutput()
        {
            Bytes.Add(OPCodes.Codes.AssignTypeRegister);
            Bytes.Add(0);
            Bytes.Add(V1.Type);

            Bytes.Add(new AssignOperationRegister(V1, 0).ByteOutput());

            Bytes.Add(OPCodes.Codes.Cast);
            Bytes.Add(V2.Type);
            Bytes.Add(V2.Address);

            return (Bytes.Convert());
        }
    }
}
