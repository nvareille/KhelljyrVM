using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KhelljyrCommon
{
    public static class Extensions
    {
        public static T As<T>(this object obj) where T : class
        {
            return (obj as T);
        }

        public static byte[] ToByteArray(this string str)
        {
            return (str.Select(i => (byte)i).ToArray());
        }
    }
}
