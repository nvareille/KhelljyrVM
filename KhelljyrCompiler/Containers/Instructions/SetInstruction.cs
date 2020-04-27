using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class NewSetInstruction : Instruction
    {
        public Variable Source;
        public Variable Destination;

        public override byte[] ByteOutput()
        {
            Bytes.Add(new AssignTargetRegisterInstruction(Source, 0));
            Bytes.Add(new AssignTargetRegisterInstruction(Destination, 1));

            if (Source is ConstPtrVariable)
                Bytes.Add(OPCodes.Codes.AssignPtrCarry);

            Bytes.Add(OPCodes.Codes.Set);
            Bytes.Add(Source.Size);
            Bytes.Add(Source.GetAddressOrValue());
            Bytes.Add(Destination.GetAddressOrValue());
            
            return (Bytes.Convert());
        }
    }
}
