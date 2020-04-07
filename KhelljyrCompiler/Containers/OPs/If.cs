using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCompiler.Containers;
using KhelljyrCompiler.Containers.Instructions;

namespace KhelljyrCompiler
{
    public partial class OPInterpretor
    {
        public static Dictionary<string, ConditionFlag> Flags = new Dictionary<string, ConditionFlag>
        {
            {"==", ConditionFlag.Equals},
            {"!=", ConditionFlag.NotEquals},
            {"<", ConditionFlag.Lower},
            {"<=", ConditionFlag.LowerEquals},
            {">", ConditionFlag.Greater},
            {">=", ConditionFlag.GreaterEquals},
        };

        public static void If(Compiler cmp, string[] args)
        {
            Function fct = cmp.Functions.Last();
            IfInstruction i = new IfInstruction();

            i.V1 = ArgumentSelector.ExtractOrder<Variable>(ArgumentSelector.Const_Variable, fct, args[1]);
            i.V2 = ArgumentSelector.ExtractOrder<Variable>(ArgumentSelector.Const_Variable, fct, args[3]);
            i.Flag = Flags[args[2]];
            i.Label = args[4];

            fct.Instructions.Add(i);
        }
    }
}
