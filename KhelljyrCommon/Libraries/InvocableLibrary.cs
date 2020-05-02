using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace KhelljyrCommon.Libraries
{
    public class InvocableLibrary
    {
        public KhelljyrLibrary Library;
        public List<string> AvailableMethods = new List<string>();
        public ProgramReader StackReader;
        private Dictionary<string, Action<ProgramReader>> Methods = new Dictionary<string, Action<ProgramReader>>();

        public InvocableLibrary(KhelljyrLibrary lib)
        {
            Library = lib;
            BindMethods();
        }

        private void BindMethods()
        {
            IEnumerable<MethodInfo> methods = Library.GetAvailableMethods();

            foreach (MethodInfo info in methods)
            {
                AvailableMethods.Add(info.Name);
                Methods[info.Name] = (r) =>
                {
                    info.Invoke(Library, new []{r});
                };
            }
        }

        public void Invoke(ProgramReader reader, string method)
        {
            StackReader = reader;
            Methods[method](reader);
        }
    }
}
