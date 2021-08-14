using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators.TemplateFillers
{
    public class List : ITemplateFiller
    {
        public List(dynamic j) : base((object)j) { }

        public override string render(string language)
        {
            string lang = language;

            var sb = @$"<!DOCTYPE html>
<html lang='{lang}'>
" + getHead(lang) + @"
<body>
" + getHeader(lang) + @$"
    <div class='row' id='{json.body.id}'></div>
</body>

</html>";

            return sb;
        }

        private string getHeader(string lang)
        {
            string sb = @"<div class='row notgamerow'>
    <header>";

            foreach(dynamic t in json.header)
            {
                switch((string)t.type)
                {
                    case "dropdown":
                        sb += $@"
    {getDropdown(t, lang)}";
                        break;

                    case "button":
                        sb += $@"
{getButton(t, lang)}";
                        break;

                    case "search":
                        sb += $@"
{getSearch(t, lang)}";
                        break;
                }
            }

            sb += @"
    </header>
</div>";
            return indentString(sb, @"  ");
        }

        private string getDropdown(dynamic obj, string lang)
        {
            string sb = @$"
<label class='forselect' for='{obj.id}'>{obj.text[lang]}</label>
<select id='{obj.id}'" + $" onchange=\"{obj.onchange}\">";

            foreach (dynamic s in obj.items)
            {
                sb += $@"
    <option tag='{((string)s["en"]).Replace(" ", "").Replace("(", "")}'>{s[lang]}</option>";
            }

            sb += @"
</select>";

            return indentString(sb, @"        ");
        }

        private string getButton(dynamic obj, string lang)
        {
            string sb = @"
" + $"<span data-tooltip='{obj.tooltip[lang]}' onclick=\"{obj.onclick}\" style='cursor: pointer;' class='forselect material-icons'>{obj.icon}</span>";
            return indentString(sb, @"        ");
        }

        private string getSearch(dynamic obj, string lang)
        {
            string sb = @"
<div class='form__group field'>
    <input " + $"oninput=\"{obj.oninput}\" type='input' class='form__field' id='{obj.id}' placeholder='-' required />" + @$"
    <label for='{obj.id}' class='form__label'>{obj.text[lang]}</label>
</div>
<br>";
            return indentString(sb, @"        ");
        }
    }
}
