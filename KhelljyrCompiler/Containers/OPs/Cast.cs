using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCompiler.Containers;
using KhelljyrCompiler.Containers.Instructions;

namespace KhelljyrCompiler
{
    public partial class OPInterpretor
    {
        private static void Cast(Compiler cmp, string[] args)
        {
            Function fct = cmp.Functions.Last();
            Variable v1 = ArgumentSelector.ExtractVar(fct, args[1]);
            Variable v2 = ArgumentSelector.ExtractVar(fct, args[2]);

            CastInstruction c = new CastInstruction(v1, v2);

            fct.Instructions.Add(c);
        }
    }
}
