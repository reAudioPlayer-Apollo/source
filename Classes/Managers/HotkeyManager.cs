using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHotkey.WindowsForms;

namespace reAudioPlayerML
{
    public class HotkeyManager
    {
        MediaPlayer media;
        MetroFramework.Controls.MetroTrackBar volumeBar;

        public HotkeyManager(MediaPlayer mediaPlayer, MetroFramework.Controls.MetroTrackBar volume)
        {
            media = mediaPlayer;
            volumeBar = volume;

            try
            {
                NHotkey.WindowsForms.HotkeyManager.Current.AddOrReplace("PlayPause", System.Windows.Forms.Keys.NumPad5, playPauseHandler);
                NHotkey.WindowsForms.HotkeyManager.Current.AddOrReplace("VolUp", System.Windows.Forms.Keys.NumPad8, volUpHandler);
                NHotkey.WindowsForms.HotkeyManager.Current.AddOrReplace("VolDown", System.Windows.Forms.Keys.NumPad2, volDownHandler);
                NHotkey.WindowsForms.HotkeyManager.Current.AddOrReplace("Next", System.Windows.Forms.Keys.NumPad6, nextHandler);
                NHotkey.WindowsForms.HotkeyManager.Current.AddOrReplace("Last", System.Windows.Forms.Keys.NumPad4, lastHandler);
            } catch { }
        }

        private void playPauseHandler(object sender, NHotkey.HotkeyEventArgs e)
        {
            media.playPause();
            e.Handled = true;
        }

        private void volUpHandler(object sender, NHotkey.HotkeyEventArgs e)
        {
            if (volumeBar.Value < volumeBar.Maximum)
                volumeBar.Value += 2;
            e.Handled = true;
        }

        private void volDownHandler(object sender, NHotkey.HotkeyEventArgs e)
        {
            if (volumeBar.Value > 0)
                volumeBar.Value -= 2;
            e.Handled = true;
        }

        private void nextHandler(object sender, NHotkey.HotkeyEventArgs e)
        {
            media.next();
            e.Handled = true;
        }

        private void lastHandler(object sender, NHotkey.HotkeyEventArgs e)
        {
            media.last();
            e.Handled = true;
        }
    }
}
