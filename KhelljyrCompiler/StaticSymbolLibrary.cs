using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KhelljyrCompiler
{
    public class StaticSymbolLibrary
    {
        public Dictionary<string, uint> Symbols = new Dictionary<string, uint>();
        public int TableSize;

        public uint AddSymbol(string str)
        {
            uint addr = 0;

            if (!Symbols.ContainsKey(str))
            {
                if (Symbols.Any())
                {
                    var a = Symbols.Last();

                    addr = a.Value + (uint)a.Key.Length + 1;
                }

                Symbols.Add(str, addr);
                TableSize = Symbols.Sum(i => i.Key.Length + 1);
            }

            return (addr);
        }
    }
}
