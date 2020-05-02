using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon.Libraries;

namespace KhelljyrCommon
{
    public class LibStackContainer : StackContainer
    {
        public string Library;
        public string Function;

        public LibStackContainer(string lib, string fct, RangeContainer memory) : base(memory, 0)
        {
            Library = lib;
            Function = fct;
        }

        public int Invoke(LibraryHandler lh, ProgramReader r)
        {
            lh.Invoke(r, Library, Function);

            return (r.Elapsed());
        }
    }

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
