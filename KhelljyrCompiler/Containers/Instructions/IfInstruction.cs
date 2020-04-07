using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class IfInstruction : Jumpable
    {
        public Variable V1;
        public Variable V2;
        public ConditionFlag Flag;

        public override byte[] ByteOutput()
        {
            List<byte[]> bytes = new List<byte[]>();

            bytes.Add(TreatVariable(V1, 0));
            bytes.Add(TreatVariable(V2, 1));
            
            bytes.Add(GetBytes(OPCodes.Codes.If));
            bytes.Add(GetBytes((int)Flag));

            bytes.Add(GetBytes(0));

            return (Convert(bytes));
        }

        private byte[] TreatVariable(Variable v, int id)
        {
            List<byte[]> bytes = new List<byte[]>();

            if (v is IConstVariable)
            {
                bytes.Add(GetBytes(OPCodes.Codes.AssignStaticConditionRegister));
                bytes.Add(GetBytes(id));
                bytes.Add(v.As<IConstVariable>().GetValueAsBytes());
            }
            else
            {
                bytes.Add(GetBytes(OPCodes.Codes.AssignConditionRegister));
                bytes.Add(GetBytes(id));
                bytes.Add(GetBytes(v.Address));
            }

            return (Convert(bytes));
        }

        public override int LocalAddressToWrite()
        {
            return (Defines.SIZE_INT * 9);
        }
    }
}
