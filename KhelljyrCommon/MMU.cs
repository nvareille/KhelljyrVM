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
            End = start + size - 1;
            Memory = new byte[size];
        }

        public bool ContainsAddress(uint dest)
        {
            return (Start <= dest && End >= dest);
        }

        public void Write(byte[] b)
        {
            Array.Copy(b, Memory, b.Length);
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
            Last = null;
        }

        public void Free(int start)
        {
            int idx = Segments.FindIndex(i => i.Start == start);

            Segments.RemoveAt(idx);
            Last = null;
        }

        private void ComputeLast(uint dest)
        {
            try
            {
                if (Last == null || !Last.ContainsAddress(dest))
                    Last = Segments.First(i => i.Start <= dest && i.End >= dest);
            }
            catch (Exception e)
            {
                throw new Exception("Segmentation Fault");
            }
            
        }

        public void WriteByte(byte b, uint dest)
        {
            ComputeLast(dest);

            dest -= (uint)Last.Start;
            Last.Memory[dest] = b;
        }

        public void WriteBytes(byte[] array, uint dest)
        {
            uint count = 0;

            foreach (byte b in array)
            {
                WriteByte(b, dest + count);

                ++count;
            }
        }

        public byte ReadByte(uint dest)
        {
            ComputeLast(dest);

            dest -= (uint)Last.Start;
            return (Last.Memory[dest]);
        }

        public byte[] ReadBytes(uint dest, int length)
        {
            int count = 0;
            byte[] array = new byte[length];

            while (count < length)
            {
                array[count] = ReadByte(dest + (uint) count);
                ++count;
            }

            return (array);
        }

        public uint ReadPtr(uint dest)
        {
            byte[] b = ReadBytes(dest, Defines.SIZE_PTR);

            return (BitConverter.ToUInt32(b));
        }
    }
}
