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
            TreatVariable(V1, 0);
            TreatVariable(V2, 1);
            
            Bytes.Add(OPCodes.Codes.If);
            Bytes.Add((int)Flag);

            Bytes.Add(0);

            return (Bytes.Convert());
        }

        private void TreatVariable(Variable v, int id)
        {
            if (v is IConstVariable)
            {
                Bytes.Add(OPCodes.Codes.AssignStaticConditionRegister);
                Bytes.Add(id);
                Bytes.Add(v.As<IConstVariable>().GetValueAsBytes());
            }
            else
            {
                Bytes.Add(OPCodes.Codes.AssignConditionRegister);
                Bytes.Add(id);
                Bytes.Add(v.Address);
            }
        }

        public override int LocalAddressToWrite()
        {
            return (Defines.SIZE_INT * 9);
        }
    }
}
