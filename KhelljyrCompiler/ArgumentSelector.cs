using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCompiler.Containers;

namespace KhelljyrCompiler
{
    public enum ArgType
    {
        Const,
        Variable,
        Function
    }

    public class ArgumentSelector
    {
        public static readonly ArgType[] Const_Variable = new ArgType[] {ArgType.Const, ArgType.Variable};

        public static T ExtractOrder<T>(IEnumerable<ArgType> what, Function fct, string str) where T : Extractable
        {
            Extractable element = null;

            foreach (ArgType argType in what)
            {
                if (argType == ArgType.Const)
                    element = ExtractConst(str);

                if (argType == ArgType.Variable)
                    element = ExtractVar(fct, str);

                if (element != null)
                    return ((T)element);
            }

            return (null);
        }

        public static Variable ExtractConst(string str)
        {
            int intValue;
            Variable v = null;

            if (Int32.TryParse(str, out intValue))
                v = new ConstIntVariable(intValue);

            return (v);
        }

        public static Variable ExtractVar(Function fct, string str)
        {
            return (fct.Variables.FirstOrDefault(i => i.Name == str));
        }
    }
}
