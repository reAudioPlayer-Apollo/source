using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Generators
{
    public class HTMLGenerator
    {
        private dynamic obj;

        public HTMLGenerator(string file)
        {
            obj = JsonConvert.DeserializeObject(File.ReadAllText(file));
        }

        public string render(string language)
        {
            TemplateFillers.ITemplateFiller t;

            switch(obj.template)
            {
                case "list":
                default:
                    t = new TemplateFillers.List(obj);
                    break;
            }

            return t.render(language);
        }
    }
}
