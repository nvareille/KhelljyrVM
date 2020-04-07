using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon
{
    public static class Extensions
    {
        public static T As<T>(this object obj) where T : class
        {
            return (obj as T);
        }
    }
}
