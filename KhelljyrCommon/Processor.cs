using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KhelljyrCommon
{
    public class Processor
    {
        public StackContainer FunctionToCall;
        public StackContainer ActiveStackContainer;
        public Stack<StackContainer> Stack = new Stack<StackContainer>();
        public byte[] Program;
        public Dictionary<int, int> Functions = new Dictionary<int, int>();
        public Registers Registers = new Registers();
        public MMU MMU = new MMU();

        public bool Running;

        public void LoadProgram(byte[] prg)
        {
            int start = (LoadFunctions(prg) + 1) * Defines.SIZE_INT;

            Program = new byte[prg.Length - start];
            Array.Copy(prg, start, Program, 0, prg.Length - start);
        }

        private int LoadFunctions(byte[] prg)
        {
            int fcts = BitConverter.ToInt32(prg, 0);
            int count = 0;

            while (count < fcts)
            {
                Functions[count] = BitConverter.ToInt32(prg, (count + 1) * Defines.SIZE_INT);
                ++count;
            }

            return (fcts);
        }

        public int Run()
        {
            Running = true;
            CallFunction(0);

            while (Running)
                ExecNextOp();

            return (BitConverter.ToInt32(Registers.ReturnCarry));
        }

        private OPCodes.Codes GetOPCode(int counter)
        {
            int op = BitConverter.ToInt32(Program, counter);

            return ((OPCodes.Codes) op);
        }

        public void ExecNextOp()
        {
            Registers.JumpCarry = false;

            if (!Stack.Any())
            {
                Running = false;
                return;
            }

            ActiveStackContainer = Stack.Peek();
            int counter = ActiveStackContainer.ProgramCounter;
            OPCodes.Codes code = GetOPCode(counter);
            ProgramReader reader = new ProgramReader(this, counter);
            Func<Processor, ProgramReader, int> fct = OPCodesActions.Actions[code];

            reader.NextInt();
            counter += fct(this, reader);

            if (!Registers.JumpCarry)
                ActiveStackContainer.ProgramCounter = counter;
        }

        public int GetFunctionStackSize(int id)
        {
            int fctPtr = Functions[id];

            return (BitConverter.ToInt32(Program, fctPtr));
        }

        public void CallFunction(int id)
        {
            int stackSize = GetFunctionStackSize(id);

            PushStackFunction(stackSize, Functions[id] + Defines.SIZE_INT);
        }

        public void PushStackFunction(int stackSize, int counter)
        {
            Stack.Push(new StackContainer(MMU.Alloc(stackSize), counter));
        }
    }
}
