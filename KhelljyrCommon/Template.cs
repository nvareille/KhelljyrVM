using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon
{
    public class Template
    {
        public string Name;
        public List<string> Tokens = new List<string>();
        public string Content;
        
        public GeneratedTemplate Generate(IEnumerable<string> args)
        {
            int count = 0;
            string str = Content;
            List<string> name = new List<string> { Name };
            GeneratedTemplate template = new GeneratedTemplate();

            foreach (string s in args)
            {
                name.Add(s);
                str = str.Replace(Tokens[count], s);
                ++count;
            }

            template.Name = string.Join("_", name);
            template.Content = str.Replace(Name, template.Name);

            return (template);
        }
    }

    public class GeneratedTemplate
    {
        public string Name;
        public string Content;
    }
}
