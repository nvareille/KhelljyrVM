using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon
{
    public class ProgramReader
    {
        private int Counter;
        private int StartCounter;
        private byte[] Program;

        public ProgramReader(Processor proc, int counter)
        {
            Program = proc.Program;
            StartCounter = counter;
            SetProgramCounter(counter);
        }

        public ProgramReader(byte[] program, int counter)
        {
            Program = program;
            Counter = counter;
            StartCounter = counter;
        }

        public int Elapsed()
        {
            return (Counter - StartCounter);
        }

        public int GetCounter()
        {
            return (Counter);
        }

        public void SetProgramCounter(int counter)
        {
            Counter = counter;
        }

        public int MoveCounter(int counter)
        {
            Counter += counter;

            return (counter);
        }

        public int NextInt()
        {
            int value = BitConverter.ToInt32(Program, Counter);

            MoveCounter(Defines.SIZE_INT);

            return (value);
        }

        public byte NextByte()
        {
            byte value = Program[Counter];

            MoveCounter(Defines.SIZE_BYTE);

            return (value);
        }

        public uint NextPtr()
        {
            uint value = BitConverter.ToUInt32(Program, Counter);

            MoveCounter(Defines.SIZE_PTR);

            return (value);
        }

        public byte[] NextArray(int size)
        {
            byte[] array = new byte[size];

            Array.Copy(Program, Counter, array, 0, size);
            MoveCounter(size);
            return (array);
        }

        public bool IsOver()
        {
            return (Counter >= Program.Length);
        }

        public string NextString()
        {
            StringBuilder str = new StringBuilder();
            byte b = NextByte();
            
            while (b != 0)
            {
                str.Append((char)b);
                b = NextByte();
            }

            return (str.ToString());
        }
    }
}
