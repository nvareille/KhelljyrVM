using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon
{
    public class Registers
    {
        public bool JumpCarry;
        public bool PtrCarry;
        public byte[] ReturnCarry = new byte[Defines.SIZE_PTR];
        public TypeFlag[] TypeRegisters = new TypeFlag[2];
        public byte[][] OperationRegisters = new byte[][]
        {
            new byte[Defines.MAX_SIZE],
            new byte[Defines.MAX_SIZE],
            new byte[Defines.MAX_SIZE],
        };

        public byte[][] ConditionRegisters = new byte[][]
        {
            new byte[Defines.SIZE_INT],
            new byte[Defines.SIZE_INT],
        };

        public TargetFlag[] TargetRegisters = new TargetFlag[5];

        public void SetReturnCarry(byte[] array, uint idx, int size)
        {
            Array.Copy(array, idx, ReturnCarry, 0, size);
        }

        public void SetReturnCarry(int value)
        {
            SetReturnCarry(BitConverter.GetBytes(value), 0, Defines.SIZE_INT);
        }
    }
}
