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
        public static void SetAppDomainCultures(string name)
        {
            try
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(name);
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(name);
            }
            // If an exception occurs, we'll just fall back to the system default.
            catch (CultureNotFoundException)
            {
                return;
            }
            catch (ArgumentException)
            {
                return;
            }
        }

        public static void SetAppDomainCultures(string[] names)
        {
            SetAppDomainCultures(names[0]);
        }

        public static void SetCulture(string culture, Thread t)
        {
            try
            {
                var cultureInfo = CultureInfo.GetCultureInfo(culture);
                Application.CurrentCulture = cultureInfo;
                t.CurrentCulture = cultureInfo;
                t.CurrentUICulture = cultureInfo;
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            Debug.WriteLine("test");
        }
    }
}
