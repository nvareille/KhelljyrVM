using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon.OPCalls
{
    public class SysCalls
    {
        public static int Exit(Processor proc, ProgramReader reader)
        {
            proc.Running = false;

            return (reader.Elapsed());
        }
    }
}
