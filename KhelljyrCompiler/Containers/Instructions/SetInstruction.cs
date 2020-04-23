using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class SetIntInstruction : SetInstruction<int>
    {
        public SetIntInstruction() : base(Defines.SIZE_INT) { }

        public override byte[] GetByteValue()
        {
            return (BitConverter.GetBytes(Value));
        }
    }

    public class SetFloatInstruction : SetInstruction<float>
    {
        public SetFloatInstruction() : base(Defines.SIZE_FLOAT) { }

        public override byte[] GetByteValue()
        {
            return (BitConverter.GetBytes(Value));
        }
    }

    public abstract class SetInstruction<T> : Instruction
    {
        public Variable Variable;
        public T Value;
        public int Size;

        protected SetInstruction(int size)
        {
            Size = size;
        }

        public abstract byte[] GetByteValue();

        public override byte[] ByteOutput()
        {
            List<byte[]> bytes = new List<byte[]>();

            bytes.Add(GetBytes(OPCodes.Codes.AssignStatic));
            bytes.Add(GetBytes(Variable.Address));
            bytes.Add(GetBytes(Size));
            bytes.Add(GetByteValue());

            return (Convert(bytes));
        }
    }
}
