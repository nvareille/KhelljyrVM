using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrVM
{
    public class CompilationOptions
    {
        public string Output = "out.khl";
        public IEnumerable<string> Files = new List<string>();
    }
}
