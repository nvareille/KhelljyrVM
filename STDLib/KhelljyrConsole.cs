using System;
using KhelljyrCommon;
using KhelljyrCommon.Libraries;
using KhelljyrLibrary;

namespace STDLib
{
    public class KhelljyrConsole : KhelljyrCommon.Libraries.KhelljyrLibrary
    {
        private Processor Processor;

        public KhelljyrConsole(LibraryHandler handler) : base(handler) { }

        public override void Init()
        {
            Processor = LibraryHandler.GetResource<Processor>();
        }

        [LibraryFunction]
        public void Print(ProgramReader reader)
        {
            Pointer ptr = new Pointer(Processor.MMU)
            {
                Value = reader.NextPtr()
            };

            string str = ptr.GetString();
            Console.WriteLine(str);
        }
        
        [LibraryFunction]
        public void ReadInt(ProgramReader reader)
        {
            string str = Console.ReadLine();
            int value = Int32.Parse(str);

            Processor.Registers.SetReturnCarry(value);
        }
    }
}
