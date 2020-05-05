using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrLibrary
{
    public class Pointer
    {
        private MMU MMU;
        public uint Value;

        public Pointer(MMU mmu)
        {
            MMU = mmu;
        }

        public string GetString()
        {
            StringBuilder builder = new StringBuilder();
            uint count = Value;
            byte b = MMU.ReadByte(count);

            while (b != 0)
            {
                builder.Append((char)b);
                b = MMU.ReadByte(++count);
            }

            return (builder.ToString());
        }
    }
}
