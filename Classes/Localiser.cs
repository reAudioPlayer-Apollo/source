using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML.Classes
{
    public static class Localiser
    {
        private static CultureInfo GetCulture(string culture)
        {
            switch(culture.ToLower())
            {
                case "deutsch":
                case "de":
                    return CultureInfo.GetCultureInfo("de");
                case "français":
                case "fr":
                    return CultureInfo.GetCultureInfo("fr");
                case "português":
                case "pt":
                    return CultureInfo.GetCultureInfo("pt");
                default:
                    return CultureInfo.GetCultureInfo("en");
            }
        }

        public static void SetCulture(string culture, ml main)
        {
            var cultureInfo = GetCulture(culture);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            var gen = new Generators.HTMLGenerator(@"resources\www\library\generator.refe.json");
            var file = gen.render(cultureInfo.IetfLanguageTag);
            File.WriteAllText(@"resources\www\library\index.html", file);

            main.cacheStates();
            main.Controls.Clear();
            main.init();
        }
    }
}
