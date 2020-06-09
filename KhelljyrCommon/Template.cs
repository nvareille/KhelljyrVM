using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
            template.Content = str;
            template.Content = Regex.Replace(str, "([^a-zA-Z0-9]|[ \t]+|^)" + Name, " " + template.Name);

            return (template);
        }
    }

    public class GeneratedTemplate
    {
        public string Name;
        public string Content;
    }
}
