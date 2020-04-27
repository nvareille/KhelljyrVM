using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon.MemoryOperators
{
    public class ConstMemoryReader : MemoryReader
    {
        public ConstMemoryReader(byte[] bytes)
        {
            Data = bytes;
        }
    }

    public class AddressMemoryReader : MemoryReader
    {
        public AddressMemoryReader(RangeContainer container, uint ptr, int size)
        {
            Data = new byte[size];

            Array.Copy(container.Memory, ptr, Data, 0, size);
        }
    }

    public class PtrMemoryReader : MemoryReader
    {
        public PtrMemoryReader(RangeContainer container, MMU mmu, uint ptr, int size)
        {
            ptr = BitConverter.ToUInt32(container.Memory, (int) ptr);
            Data = mmu.ReadBytes(ptr, size);
        }
    }

    public class MemoryReader
    {
        public byte[] Data;

        public static MemoryReader GetReader(int idx, Processor proc, byte[] source, int sourceSize)
        {
            if (proc.Registers.TargetRegisters[idx] == TargetFlag.Const)
            {
                if (proc.Registers.PtrCarry)
                {
                    int a = BitConverter.ToInt32(source) + proc.ActiveStackContainer.Memory.Start;

                    source = BitConverter.GetBytes(a);
                    proc.Registers.PtrCarry = false;
                }
                return (new ConstMemoryReader(source));
            }

            if (proc.Registers.TargetRegisters[idx] == TargetFlag.Address)
                return (new AddressMemoryReader(proc.ActiveStackContainer.Memory, (uint)BitConverter.ToInt32(source), sourceSize));

            if (proc.Registers.TargetRegisters[idx] == TargetFlag.Ptr)
                return (new PtrMemoryReader(proc.ActiveStackContainer.Memory, proc.MMU, (uint)BitConverter.ToInt32(source), sourceSize));

            return (null);
        }
    }
}
