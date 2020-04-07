using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCompiler.Containers
{
    public class ScalarFinder
    {
        public static void TryGetInt(string str, Action<int> act)
        {
            int value = 0;

            if (Int32.TryParse(str, out value))
            {
                act(value);
            }
        }
    }
}
