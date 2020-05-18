using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Semantics;
using KhelljyrCompiler.Containers;
using KhelljyrCompiler.Containers.Instructions;
using Newtonsoft.Json;

namespace KhelljyrCompiler
{
    public partial class OPInterpretor
    {
        private static void Struct(Compiler cmp, Argument[] args)
        {
            Structure strct = new Structure();

            strct.Name = args[1];

            cmp.Structures.Add(strct);
            cmp.ProcessingBlock = strct;

            cmp.OPInterpretor.Fcts.Add(strct.Name, StructureImplementation(strct));
        }

        private static Action<Compiler, Argument[]> StructureImplementation(Structure s)
        {
            Action<Compiler, Argument[]> act = (cmp, args) =>
            {
                Function f = cmp.Functions.Last();

                s.Variables.ForEach(o =>
                {
                    Variable v = (Variable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(o), o.GetType());

                    v.Name = args[1] + "." + v.Name;

                    f.AddVariable(v);
                });
            };

            return (act);
        }
    }
}
