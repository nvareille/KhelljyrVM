using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon
{
    public static class Defines
    {
        public const int SIZE_INT = 4;
        public const int SIZE_FLOAT = 4;
        public const int SIZE_PTR = 4;
        public const int MAX_SIZE = 8;
        public class Array
        {
            public int Size;

            public Array(int size)
            {
                Size = size;
            }
        }
    }
}
