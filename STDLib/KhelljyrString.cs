using System;
using System.Linq;
using System.Text.RegularExpressions;
using KhelljyrCommon;
using KhelljyrCommon.Libraries;
using KhelljyrLibrary;

namespace STDLib
{
    public class KhelljyrString : KhelljyrCommon.Libraries.KhelljyrLibrary
    {
        private Processor Processor;

        public KhelljyrString(LibraryHandler handler) : base(handler) { }

        public override void Init()
        {
            Processor = LibraryHandler.GetResource<Processor>();
        }

        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);

            if (pos < 0)
            {
                return text;
            }

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        [LibraryFunction]
        public void FormatString(ProgramReader reader)
        {
            Pointer ptr = new Pointer(Processor.MMU)
            {
                Value = reader.NextPtr()
            };

            string str = ptr.GetString();

            Regex r = new Regex(@"\{%[d,s]}");
            MatchCollection matches = r.Matches(str);

            foreach (Match match in matches)
            {
                object obj = null;

                if (match.Value == "{%d}")
                    obj = reader.NextInt();
                if (match.Value == "{%s}")
                {
                    obj = new Pointer(Processor.MMU)
                    {
                        Value = reader.NextPtr()
                    }.GetString();

                }

                str = ReplaceFirst(str, match.Value, obj.ToString());
            }

            RangeContainer c = Processor.MMU.Alloc(str.Length + 1);

            c.Write(str.Select(i => (byte) i).ToArray());
            Processor.Registers.SetReturnCarry(c.Start);
        }
    }
}
