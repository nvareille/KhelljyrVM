using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KhelljyrCommon
{
    public class RangeContainer
    {
        public int Start;
        public int End;
        public byte[] Memory;

        public RangeContainer(int start, int size)
        {
            Start = start;
            End = start + size;
            Memory = new byte[size];
        }
    }

    public class MMU
    {
        public List<RangeContainer> Segments = new List<RangeContainer>();

        public RangeContainer Last;

        public RangeContainer Alloc(int size)
        {
            int start = 0;

            if (Segments.Any())
                start = Segments.Last().End + 1;
            
            RangeContainer container = new RangeContainer(start, size);

            Last = container;
            Segments.Add(container);

            return (container);
        }

        public void Free(RangeContainer container)
        {
            Segments.Remove(container);
        }

        public void Free(int start)
        {
            int idx = Segments.FindIndex(i => i.Start == start);

            Segments.RemoveAt(idx);
        }
    }
}
