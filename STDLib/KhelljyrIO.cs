using System;
using KhelljyrCommon;
using KhelljyrCommon.Libraries;

namespace STDLib
{
    public class KhelljyrIO : KhelljyrLibrary
    {
        public KhelljyrIO(LibraryHandler handler) : base(handler) { }

        [LibraryFunction]
        public void Test(ProgramReader reader)
        { 
            int a = reader.NextInt();
            int b = reader.NextInt();
            int c = reader.NextInt();

            a = a + b + c;
            LibraryHandler.GetResource<Processor>().Registers.SetReturnCarry(a);
        }
    }
}
