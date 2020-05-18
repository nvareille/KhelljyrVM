using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Semantics;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class FunctionRetInstruction : Instruction
    {
        public Function FunctionToCall;
        public List<Variable> Variables = new List<Variable>();

        public override byte[] ByteOutput()
        {
            FctCallRetInstruction i = new FctCallRetInstruction(FunctionToCall);

            i.Variables = Variables;
            Bytes.Add(i.ByteOutput());

            return (Bytes.Convert());
        }
    }
}
