using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KhelljyrCommon.Libraries
{
    public class LibraryHandler
    {
        public List<object> Resources = new List<object>();
        public List<InvocableLibrary> LoadedLibraries = new List<InvocableLibrary>();

        public LibraryHandler(params object[] objs)
        {
            Resources.AddRange(objs);
        }

        public void LoadLibrary(string path)
        {
            Assembly a = Assembly.LoadFrom(path);

            IEnumerable<Type> b = a.GetTypes().Where(i => i.IsSubclassOf(typeof(KhelljyrCommon.Libraries.KhelljyrLibrary)));

            foreach (Type type in b)
            {
                KhelljyrLibrary lib = (KhelljyrLibrary)Activator.CreateInstance(type, new object[]
                {
                    this
                });

                lib.Init();
                LoadedLibraries.Add(new InvocableLibrary(lib));
            }
        }

        public void LoadLibraries(IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                LoadLibrary(path);
            }
        }

        public T GetResource<T>()
        {
            return (Resources.OfType<T>().First());
        }

        public void Invoke(ProgramReader reader, string lib, string method)
        {
            LoadedLibraries.First(i => i.Library.Name == lib).Invoke(reader, method);
        }
    }
}
