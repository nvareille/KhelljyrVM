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

        public void LoadLibrary(string path, bool runtime)
        {
            Assembly a = Assembly.LoadFrom(path);

            IEnumerable<Type> b = a.GetTypes().Where(i => i.IsSubclassOf(typeof(KhelljyrLibrary)));

            foreach (Type type in b)
            {
                KhelljyrLibrary lib = (KhelljyrLibrary)Activator.CreateInstance(type, new object[]
                {
                    this
                });

                if (runtime)
                    lib.Init();
                LoadedLibraries.Add(new InvocableLibrary(lib));
            }
        }

        public void LoadLibraries(IEnumerable<string> paths, bool runtime)
        {
            foreach (string path in paths)
            {
                LoadLibrary(path, runtime);
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
