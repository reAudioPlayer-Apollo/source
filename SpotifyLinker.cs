using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public partial class SpotifyLinker : Form
    {
        public EventHandler<Uri> onNavigateComplete;

        public SpotifyLinker(Uri uri)
        {
            InitializeComponent();
            wBrowser.Url = uri;
        }

        private void wBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        private void wBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            onNavigateComplete(wBrowser, wBrowser.Url);
        }

        private void SpotifyLinker_Load(object sender, EventArgs e)
        {

        }
    }
}
