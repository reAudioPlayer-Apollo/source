using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public partial class RevealedStream : Form
    {
        public const string defaultLink = "http://tink.ga/rev24-7/";

        public RevealedStream(string link = defaultLink)
        {
            InitializeComponent();

            webBrowser1.Url = new Uri(link);
        }

        public string getLink()
        {
            try
            {
                return webBrowser1.Url.AbsoluteUri;
            } catch { return null; }
        }

        private void RevealedStream_Load(object sender, EventArgs e)
        {
            update();
        }

        private void update()
        {
            int BrowserVer, RegVal;

            // get the installed IE version
            using (WebBrowser Wb = new WebBrowser())
                BrowserVer = Wb.Version.Major;

            // set the appropriate IE version
            if (BrowserVer >= 11)
                RegVal = 11001;
            else if (BrowserVer == 10)
                RegVal = 10001;
            else if (BrowserVer == 9)
                RegVal = 9999;
            else if (BrowserVer == 8)
                RegVal = 8888;
            else
                RegVal = 7000;

            // set the actual key
            using (RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree))
                if (Key.GetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe") == null)
                    Key.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", RegVal, RegistryValueKind.DWord);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (var bmp = ControlExtensions.DrawToImage(webBrowser1))
            {
                var w = bmp.Width;
                var h = bmp.Height;

                PlayerManager.cover = bmp.Clone() as Image;
                bmp.Dispose();
            }

            //Debug.WriteLine("bitmap saved");

            //ImageToText imageToText = new ImageToText();
        }
    }
}
