using System.Windows.Forms;

namespace reAudioPlayerML.Search.Spotify
{
    public static class UIHandler
    {
        public static ListView releaseView;
        public static ListView syncView;

        public static ContextMenuStrip ctxRelease;
        public static ContextMenuStrip ctxSync;

        public static ToolStripComboBox ctxSyncPlaylists;
        public static ToolStripComboBox mainPlaylists;

        public static TextBox txtArtistDelimiter;
        public static TextBox txtSyncIn;
        public static TextBox txtSyncOut;

        public static ComboBox cmbSyncPlaylist;

        public static Label lblSyncProgress;

        public static NotifyIcon notifyIcon;

        public static void SetBuffers()
        {
            releaseView
                .GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(releaseView, true, null);

            syncView
                .GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(syncView, true, null);
        }
    }
}
