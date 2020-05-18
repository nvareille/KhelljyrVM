using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Semantics;

namespace KhelljyrCompiler.Containers.Instructions
{
    public class VariableRetInstruction : Instruction
    {
        public Variable Variable;

        public VariableRetInstruction(Variable v)
        {
            Variable = v;
        }

        public override byte[] ByteOutput()
        {
            if (Variable is IConstVariable)
            {
                Bytes.Add(OPCodes.Codes.SetReturnCarry);
                Bytes.Add(Variable.Size);
                Bytes.Add(Variable.As<IConstVariable>().GetValueAsBytes());
            }
            else
            {
                Bytes.Add(OPCodes.Codes.Ret);
                Bytes.Add(Variable.Address);
                Bytes.Add(Defines.SIZE_INT);
            }

            Bytes.Add(OPCodes.Codes.FctPop);

            return (Bytes.Convert());
        }
    }
}
