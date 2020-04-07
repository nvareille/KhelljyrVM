using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon.OPCalls
{
    public static class Functions
    {
        public static int FunctionCall(Processor proc, ProgramReader reader)
        {
            int fctId = reader.NextInt();
            int idx = proc.Functions[fctId];
            int size = proc.GetFunctionStackSize(fctId);

            proc.FunctionToCall = new StackContainer(proc.MMU.Alloc(size), idx + Defines.SIZE_INT);

            return (reader.Elapsed());
        }

        public static int FunctionStart(Processor proc, ProgramReader reader)
        {
            proc.Stack.Push(proc.FunctionToCall);
            proc.FunctionToCall = null;
            
            return (reader.Elapsed());
        }

        public static int Return(Processor proc, ProgramReader reader)
        {
            uint ptr = reader.NextPtr();
            int size = reader.NextInt();

            proc.Registers.SetReturnCarry(proc.ActiveStackContainer.Memory.Memory, ptr, size);

            return (reader.Elapsed());
        }

        public static int FunctionPop(Processor proc, ProgramReader reader)
        {
            StackContainer c = proc.Stack.Peek();

            proc.MMU.Free(c.Memory);
            proc.Stack.Pop();

            return (reader.Elapsed());
        }

        public static int VarFctCopy(Processor proc, ProgramReader reader)
        {
            int size = reader.NextInt();
            uint from = reader.NextPtr();
            uint to = reader.NextPtr();
            StackContainer c = proc.Stack.Peek();

            Array.Copy(c.Memory.Memory, from, proc.FunctionToCall.Memory.Memory, to, size);
            
            return (reader.Elapsed());
        }

        public static int VarConstCopy(Processor proc, ProgramReader reader)
        {
            int size = reader.NextInt();
            byte[] from = reader.NextArray(size);
            uint to = reader.NextPtr();

            Array.Copy(from, 0, proc.FunctionToCall.Memory.Memory, to, size);

            return (reader.Elapsed());
        }
    }
}
