using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon.MemoryOperators
{
    public class AddressMemoryWriter : MemoryWriter
    {
        private RangeContainer Container;
        private uint Dest;

        public AddressMemoryWriter(RangeContainer container, uint dest)
        {
            Container = container;
            Dest = dest;
        }

        public override void Write(byte[] data)
        {
            Array.Copy(data, 0, Container.Memory, Dest, data.Length);
        }
    }

    public class PtrMemoryWriter : MemoryWriter
    {
        private MMU MMU;
        private uint Dest;

        public PtrMemoryWriter(RangeContainer container, MMU mmu, uint dest)
        {
            dest = BitConverter.ToUInt32(container.Memory, (int)dest);

            MMU = mmu;
            Dest = dest;
        }

        public override void Write(byte[] data)
        {
            MMU.WriteBytes(data, Dest);
        }
    }

    public abstract class MemoryWriter
    {
        public abstract void Write(byte[] data);

        public static MemoryWriter GetWriter(int idx, Processor proc, uint dest)
        {
            if (proc.Registers.TargetRegisters[idx] == TargetFlag.Address)
                return (new AddressMemoryWriter(proc.ActiveStackContainer.Memory, dest));

            if (proc.Registers.TargetRegisters[idx] == TargetFlag.Ptr)
                return (new PtrMemoryWriter(proc.ActiveStackContainer.Memory, proc.MMU, dest));

            return (null);
        }
    }
}
