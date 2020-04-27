using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCompiler.Containers;

namespace KhelljyrCommon
{
    public class ByteContainer
    {
        public List<byte[]> Bytes = new List<byte[]>();
        
        public void Add(int c)
        {
            Bytes.Add(BitConverter.GetBytes(c));
        }

        public void Add(OPCodes.Codes code)
        {
            Bytes.Add(BitConverter.GetBytes((int)code));
        }

        public void Add(TypeFlag code)
        {
            Bytes.Add(BitConverter.GetBytes((int)code));
        }

        public void Add(TargetFlag code)
        {
            Bytes.Add(new byte[]
            {
                (byte)code
            });
        }

        public void Add(byte[] bytes)
        {
            Bytes.Add(bytes);
        }

        public void Add(Instruction ins)
        {
            Bytes.Add(ins.ByteOutput());
        }

        public byte[] Convert()
        {
            return (Bytes.SelectMany(i => i).ToArray());
        }
    }
}
