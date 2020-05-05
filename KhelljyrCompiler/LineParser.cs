using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KhelljyrCompiler
{
    public enum ArgumentType
    {
        Token,
        Quoted
    }

    public class Argument
    {
        public string Value;
        public ArgumentType Type;

        public static implicit operator string(Argument a)
        {
            return (a.Value);
        }
    }

    public static class LineParser
    {
        public static string[] StringArray(this Argument[] a)
        {
            return (a.Select(i => i.Value).ToArray());
        }

        public static Argument[] GetArgs(string line)
        {
            int count = 0;
            bool quote = false;
            bool wasQuote = false;
            bool disabled = false;
            List<Argument> strs = new List<Argument>();
            StringBuilder b = new StringBuilder();

            while (count < line.Length)
            {
                char c = line[count];

                if (c == '"' && !quote)
                {
                    quote = true;
                    wasQuote = true;
                }
                else if (c == '"' && quote)
                    quote = false;
                else if (!quote)
                {
                    if (c == ' ' || c == '\t')
                    {
                        strs.Add(new Argument
                        {
                            Value = b.ToString(),
                            Type = wasQuote ? ArgumentType.Quoted : ArgumentType.Token
                        });
                        b.Clear();
                        wasQuote = false;
                    }
                    else if (c == '/' && line[count + 1] == '/')
                    {
                        disabled = true;
                    }
                    else if (!disabled)
                        b.Append(c);
                }
                else if (!disabled)
                    b.Append(c);

                ++count;
            }

            if (strs.Count != 0)
            {
                strs.Add(new Argument
                {
                    Value = b.ToString(),
                    Type = wasQuote ? ArgumentType.Quoted : ArgumentType.Token
                });
            }

            strs.RemoveAll(i => string.IsNullOrEmpty(i.Value));

            return (strs.ToArray());
        }
    }
}
