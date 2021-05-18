using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
                    return CultureInfo.GetCultureInfo("de");
                case "français":
                    return CultureInfo.GetCultureInfo("fr");
                case "português":
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

            main.cacheStates();
            main.Controls.Clear();
            main.init();
        }
    }
}
