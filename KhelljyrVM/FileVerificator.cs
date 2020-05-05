using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KhelljyrVM
{
    public static class FileVerificator
    {
        public static void EnsureFolders()
        {
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;

            path = Path.GetDirectoryName(path);
            path = Path.Combine(path, "Libs");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
