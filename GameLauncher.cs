using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Permissions;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;

namespace reAudioPlayerML
{
    public partial class GameLauncher : Form
    {
        GameChecker gameChecker;
        HttpWebServer webServer;
        int activeScannings = 0;
        FolderBrowserDialog fbd;
        Dictionary<string, GameStartInfo> installedGames = new Dictionary<string, GameStartInfo>();
        public string steamLocation;

        public string getInstalledGamesAsJSON()
        {
            var items = GameChecker.loadJson();
            List<GameChecker.GameObj> returnItems = new List<GameChecker.GameObj>();

            foreach (var item in items)
            {
                if (installedGames.ContainsKey(item.name))
                    returnItems.Add(item);
            }

            return JsonConvert.SerializeObject(returnItems);
        }

        public class GameStartInfo
        {
            public string location;
            public Platform platform;

            public enum Platform
            {
                EpicGames, Steam, Origin, Uplay, Unknown
            }
        }

        public GameLauncher(GameChecker gc, HttpWebServer hs)
        {
            InitializeComponent();
            gameChecker = gc;
            webServer = hs;

            var drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                scanAndAppend(drive.RootDirectory.FullName);
            }
        }

        static Thread threadold;

        private void button1_Click(object sender, EventArgs e)
        {
            Search.Game gamesearch = new Search.Game();

            if (fbd is null)
            {
                fbd = new FolderBrowserDialog();
                threadold = Thread.CurrentThread;
            }

            Debug.WriteLine(Thread.CurrentThread == threadold);

            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            scanAndAppend(fbd.SelectedPath);
        }

        private void scanAndAppend(string directory)
        {
            activeScannings++;
            metroProgressBar1.ProgressBarStyle = ProgressBarStyle.Marquee;
            metroProgressBar1.MarqueeAnimationSpeed = 10;
            lblScanProgress.Text = $"scanning for games in {activeScannings} folder(s)";
            lblScanProgress.Visible = true;
            this.Refresh();

            Task.Factory.StartNew(() => getGamesInFolder(directory)).ContinueWith(games =>
            {
                listBox1.Invoke(new Action(() =>
                {
                    foreach (var game in games.Result)
                    {
                        var name = gameChecker.getDisplayName(game);

                        if (!listBox1.Items.Contains(name))
                        {
                            GameStartInfo t = new GameStartInfo();
                            t.location = game;
                            if (game.Contains("steamapps"))
                                t.platform = GameStartInfo.Platform.Steam;
                            else
                                t.platform = GameStartInfo.Platform.Unknown;

                            listBox1.Items.Add(name);
                            installedGames.Add(name, t);
                        }
                    }
                }));

                activeScannings--;

                if (activeScannings == 0)
                {
                    metroProgressBar1.Invoke(new Action(() =>
                    {
                        metroProgressBar1.ProgressBarStyle = ProgressBarStyle.Continuous;
                        metroProgressBar1.Value = 0;
                    }));

                    lblScanProgress.Invoke(new Action(() =>
                    {
                        lblScanProgress.Text = $"finished!";
                        lblScanProgress.Visible = false;
                    }));
                }
                else
                {
                    lblScanProgress.Invoke(new Action(() =>
                    {
                        lblScanProgress.Text = $"scanning for games in {activeScannings} folder(s)";
                        lblScanProgress.Visible = true;
                    }));
                }
            }).ConfigureAwait(true);
        }

        string[] getExesInFolder(string folder)
        {
            try
            {
                return Directory.GetFiles(folder, "*.exe", SearchOption.AllDirectories);
            }
            catch (UnauthorizedAccessException)
            {
                if (folder.Contains("System Volume Information"))
                    return new string[0];

                List<string> exes = new List<string>();
                Debug.WriteLine("UnauthorizedAccessException, " + folder);
                try
                {
                    var dirs = Directory.GetDirectories(folder);

                    foreach (var dir in dirs)
                    {
                        if (Directory.Exists(dir))
                            exes.AddRange(getExesInFolder(dir));
                    }

                    return exes.ToArray();
                }
                catch (UnauthorizedAccessException)
                {
                    Debug.WriteLine("Inner UnauthorizedAccessException");
                    return new string[0];
                }
            }
        }

        private List<string> getGamesInFolder(string folder)
        {
            List<string> returner = new List<string>();
            var exes = getExesInFolder(folder);
            var games = gameChecker.getKnownGames();

            foreach (var exe in exes)
            {
                if (games.Contains(Path.GetFileName(exe)))
                    returner.Add(exe);
                else if (Path.GetFileName(exe) == "steam.exe")
                    steamLocation = exe;
            }

            return returner;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string displayname = listBox1.SelectedItem.ToString();

                //launchGameByName(displayname);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void launchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1_DoubleClick(sender, e);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string displayname = listBox1.SelectedItem.ToString();
            string game = installedGames[displayname].location;
            Process.Start("explorer", "/select," + game);
        }

        [Obsolete("class is deprecated, use GameLibraryManager instead.", true)]
        public bool launchGameByIGDBId(int igdbId, string source = "local")
        {
            var games = GameChecker.loadJson();
            var t = games.Where(x => x.igdbId == igdbId).FirstOrDefault();

            bool isBlocked = HttpWebServer.users[source];

            if (t == null || isBlocked)
                return false;

            launchGameByName(t.name, source, true);
            return true;
        }

        [Obsolete("class is deprecated, use GameLibraryManager instead.", true)]
        public void launchGameByName(string name, string source = "local", bool fromWeb=false)
        {
            if (fromWeb)
            {
                TaskDialogPage t = new TaskDialogPage();
                t.Verification.Text = "block user if declined";
                t.Buttons.Add(TaskDialogButton.Yes);
                t.Buttons.Add(TaskDialogButton.No);
                t.DefaultButton = TaskDialogButton.Yes;
                t.Caption = "Game Launch Requested";
                t.Heading = name;
                t.Text = $"Do you want to launch {name} as requested?";
                t.Icon = TaskDialogIcon.ShieldBlueBar;

                if (TaskDialog.ShowDialog(t) == TaskDialogButton.No)
                {
                    if (t.Verification.Checked)
                    {
                        HttpWebServer.users[source] = true;
                    }

                    return;
                }
            }

            if (installedGames[name].platform == GameStartInfo.Platform.Steam)
            {
                if (steamLocation == null || !File.Exists(steamLocation))
                {
                    MessageBox.Show("This game requires steam, but steam has not been found yet. Please try again later.", "Steam Not Found Yet!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Process.Start(steamLocation);
            }

            string game = installedGames[name].location;

            Process.Start(game);
        }
    }
}
