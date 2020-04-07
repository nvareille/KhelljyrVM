using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon
{
    public class StackContainer
    {
        public int ProgramCounter;
        public RangeContainer Memory;

        public StackContainer(RangeContainer memory, int counter)
        {
            ProgramCounter = counter;
            Memory = memory;
        }

    }
}
