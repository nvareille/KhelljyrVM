using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class AssignOperationRegister : Instruction
    {
        private Variable V;
        private int Id;

        public AssignOperationRegister(Variable v, int id)
        {
            V = v;
            Id = id;
        }

        public override byte[] ByteOutput()
        {
            Bytes.Add(OPCodes.Codes.AssignOperationRegister);
            Bytes.Add(Id);
            Bytes.Add(V.Size);
            Bytes.Add(V.Address);

            return (Bytes.Convert());
        }
    }
}
