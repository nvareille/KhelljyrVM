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
            return (Defines.SIZE_INT);
        }

        public override byte[] ByteOutput()
        {
            Bytes.Add(OPCodes.Codes.Jump);
            Bytes.Add(0);

            return (Bytes.Convert());
        }
    }
}
