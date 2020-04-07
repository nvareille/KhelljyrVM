using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KhelljyrCommon.OPCalls
{
    public static class Jumps
    {
        public static int Jump(Processor proc, ProgramReader reader)
        {
            int addr = reader.NextInt();

            proc.ActiveStackContainer.ProgramCounter = addr;
            proc.Registers.JumpCarry = true;
            return (0);
        }
    }
}
