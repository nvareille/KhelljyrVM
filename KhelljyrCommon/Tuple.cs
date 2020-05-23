using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon
{
    public class Tuple<T, U, V>
    {
        public T Item1;
        public U Item2;
        public V Item3;

        public Tuple(T a, U b, V c)
        {
            Item1 = a;
            Item2 = b;
            Item3 = c;
        }
    }
}
