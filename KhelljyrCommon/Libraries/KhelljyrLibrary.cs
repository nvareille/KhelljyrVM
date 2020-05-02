using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KhelljyrCommon.Libraries
{
    public abstract class KhelljyrLibrary
    {
        public string Name;
        protected LibraryHandler LibraryHandler;

        protected KhelljyrLibrary(LibraryHandler handler)
        {
            LibraryHandler = handler;
            Name = GetType().Name;
        }

        public IEnumerable<MethodInfo> GetAvailableMethods()
        {
            var b = GetType();
            var c = b.GetMethods();
            var d = c.Where(i =>
            {
                var a = i.CustomAttributes;

                return a.Any(o => o.AttributeType == typeof(LibraryFunctionAttribute));
            });


            return (d);
        }

        public virtual void Init() { }
    }
}
