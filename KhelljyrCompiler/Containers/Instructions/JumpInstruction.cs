using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class JumpInstruction : Jumpable
    {
        public JumpInstruction(string lbl)
        {
            Label = lbl;
        }

        public override int LocalAddressToWrite()
        {
            return (Defines.SIZE_INT * 2);
        }

        public override byte[] ByteOutput()
        {
            List<byte[]> bytes = new List<byte[]>();
            
            bytes.Add(GetBytes(OPCodes.Codes.Jump));
            bytes.Add(GetBytes(0));

            return (Convert(bytes));
        }
    }
}
