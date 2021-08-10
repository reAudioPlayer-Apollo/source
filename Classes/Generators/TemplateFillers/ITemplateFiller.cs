using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators.TemplateFillers
{
    public abstract class ITemplateFiller
    {
        public dynamic json;

        public ITemplateFiller(dynamic json) { this.json = json; }

        abstract public string render(string language);

        public string getHead(string lang)
        {
            var sb = @"<head>
    <meta charset='UTF-8'/>
    <meta name='viewport' content='width=device-width,initial-scale=1.0'>
    <title>" + json.title[lang] + @"</title>
    <link href='https://fonts.googleapis.com/icon?family=Material+Icons' rel='stylesheet'>
    <link href='https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;700&display=swap' rel='stylesheet'/>
    <link href='/library/style.css' rel='stylesheet'/>
    
    <script src='/src/theme.js'/></script>";

            foreach (string script in json.scripts)
            {
                sb += @$"
    <script src={script}></script>";
            }

            sb += @"
</head>";

            return sb;
        }

        public string indentString(string input, string indent)
        {
            return string.Join("\n", input.Split("\n").Select(x => @$"{indent}{x}"));
        }
    }
}
