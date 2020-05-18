using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Semantics;
using KhelljyrCompiler.Containers;

namespace KhelljyrCompiler
{
    public partial class OPInterpretor
    {
        public static void Function(Compiler cmp, Argument[] args)
        {
            int count = 2;
            Function fct = new Function
            {
                Name = args[1],
            };

            while (args.Length > count)
            {
                IntVariable v = new IntVariable
                {
                    Name = args[count]
                };

                fct.AddVariable(v);
                ++count;
            }

            cmp.Functions.Add(fct);
            cmp.ProcessingBlock = fct;
        }
    }
}
