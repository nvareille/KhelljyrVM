using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers
{
    public abstract class Instruction
    {
        public ByteContainer Bytes = new ByteContainer();
        public Function Function;
        public int InstructionPtr;

        public abstract byte[] ByteOutput();

        protected byte[] Convert(List<byte[]> bytes)
        {
            return (bytes.SelectMany(i => i).ToArray());
        }

        protected byte[] GetBytes(OPCodes.Codes c)
        {
            return (BitConverter.GetBytes((int) c));
        }

        protected byte[] GetBytes(int c)
        {
            return (BitConverter.GetBytes(c));
        }

        public byte[] ByteOutputInternal(int idx)
        {
            InstructionPtr = idx;

            return (ByteOutput());
        }
    }
}
