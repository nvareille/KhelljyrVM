using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class AddInstruction : Instruction
    {
        public Variable[] From;
        public Variable To;

        private void TreatVariable(int id)
        {
            if (From[id] is IConstVariable)
            {
                IConstVariable v = From[1] as IConstVariable;

                Bytes.Add(OPCodes.Codes.AssignConstOperationRegister);
                Bytes.Add(id);
                Bytes.Add(From[id].Size);
                Bytes.Add(v.GetValueAsBytes());
            }
            else
            {
                Bytes.Add(OPCodes.Codes.AssignOperationRegister);
                Bytes.Add(id);
                Bytes.Add(From[id].Size);
                Bytes.Add(From[id].Address);
            }
        }

        public override byte[] ByteOutput()
        {
            List<byte[]> bytes = new List<byte[]>();

            Bytes.Add(new AssignTargetRegisterInstruction(From[0], 0));
            Bytes.Add(new AssignTargetRegisterInstruction(From[1], 1));
            Bytes.Add(new AssignTargetRegisterInstruction(To, 2));

            Bytes.Add(OPCodes.Codes.AssignTypeRegister);
            Bytes.Add(0);
            Bytes.Add(From[0].Type);

            Bytes.Add(OPCodes.Codes.AssignTypeRegister);
            Bytes.Add(1);
            Bytes.Add(From[1].Type);

            TreatVariable(0);
            TreatVariable(1);

            Bytes.Add(OPCodes.Codes.OperationAdd);

            // ATTENTION SI C'EST UN PTR
            Bytes.Add(To.Type);
            Bytes.Add(To.Address);

            return (Bytes.Convert());
        }
    }
}
