using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class FunctionRetInstruction : Instruction
    {
        public Function FunctionToCall;
        public List<Variable> Variables = new List<Variable>();

        public override byte[] ByteOutput()
        {
            List<byte[]> bytes = new List<byte[]>();
            FctCallRetInstruction i = new FctCallRetInstruction(FunctionToCall);

            i.Variables = Variables;
            bytes.Add(i.ByteOutput());

            return (Convert(bytes));
        }
    }
}
