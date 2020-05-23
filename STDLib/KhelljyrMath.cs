using System;
using System.Linq;
using System.Text.RegularExpressions;
using KhelljyrCommon;
using KhelljyrCommon.Libraries;
using KhelljyrLibrary;

namespace STDLib
{
    public class KhelljyrMath : KhelljyrCommon.Libraries.KhelljyrLibrary
    {
        private Processor Processor;
        private Random Random;

        public KhelljyrMath(LibraryHandler handler) : base(handler) { }

        public override void Init()
        {
            Processor = LibraryHandler.GetResource<Processor>();
        }

        private Random GetRand()
        {
            if (Random == null)
                Random = new Random();

            return (Random);
        }

        [LibraryFunction]
        public void Rand(ProgramReader reader)
        {
            int min = reader.NextInt();
            int max = reader.NextInt();

            min = GetRand().Next(min, max + 1);

            Processor.Registers.SetReturnCarry(min);
        }
    }
}
