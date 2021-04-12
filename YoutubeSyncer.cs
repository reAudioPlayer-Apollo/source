using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public class YoutubeSyncer
    {
        private readonly string docs;
        private readonly Search.Youtube youtube = new Search.Youtube();

        public YoutubeSyncer()
        {
            docs = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\reAudioPlayer\\Syncs";
            string legacyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\reAudioPlayer\\Syncs";

            if (!Directory.Exists(docs))
            {
                Directory.CreateDirectory(docs);
            }

            if (Directory.Exists(legacyDocs))
            {
                foreach (string dirPath in Directory.GetFiles(legacyDocs))
                {
                    string newFile = Path.Combine(docs, Path.GetFileName(dirPath));

                    if (!File.Exists(newFile))
                    {
                        File.Copy(dirPath, newFile);
                    }
                }
            }

            /*
            Search.Spotify spotify = new Search.Spotify();
            spotify.client.Search.Item(new SpotifyAPI.Web.SearchRequest(SpotifyAPI.Web.SearchRequest.Types.Album, "Blasterjaxx"));

            Search.Youtube search = new Search.Youtube();
            search.relatedToId("kDsPPr5TNAw");
            search.byQuery("Blasterjaxx", Search.Youtube.type.Channel);

            Search.Movie movie = new Search.Movie();
            movie.byQuery("La revolution");

            Search.Game game = new Search.Game();
            game.byQuery("Thief");*/
        }

        private async void FswDownload_Created(object sender, FileSystemEventArgs e)
        {
            OptimizeDL optimiser = new OptimizeDL();
            var video = wasDownloaded(e.Name);

            if (!(video is null))
            {
                //var wildcard = e.FullPath.Replace(Path.GetExtension(e.FullPath), ".*");
                while (Directory.GetFiles(Path.GetDirectoryName(e.FullPath), Path.GetFileNameWithoutExtension(e.FullPath) + ".*").Length > 1)
                {
                    await Task.Delay(500); // give it some time...
                }

                await Task.Delay(500); // give it some time...

                if (video.acrop)
                {
                    Debug.WriteLine(optimiser.acrop(new FileInfo(e.FullPath)));
                }
                if (video.splitChapters &&
                    MessageBox.Show("this video appears to have chapters, do you want to split into chapters?", "Apollo Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Debug.WriteLine(optimiser.splitByChapter(new FileInfo(e.FullPath), video.video));
                }
            }
        }

        private VideoDownload wasDownloaded(string name)
        {
            foreach (var video in downloadedMusic)
            {
                if (video.title == name)
                {
                    return video;
                }
            }

            return null;
        }

        private readonly List<FileSystemWatcher> fswDownload = new List<FileSystemWatcher>();

        public void addOptimiseWatcher(string directory)
        {
            FileSystemWatcher t = new FileSystemWatcher();
            t.Path = directory;
            t.EnableRaisingEvents = true;
            t.Created += FswDownload_Created;

            fswDownload.Add(t);
        }

        public async void sync()
        {
            string[] syncs = Directory.GetFiles(docs, "*.arch");

            try
            {
                foreach (string sync in syncs)
                {
                    string dir = Path.GetDirectoryName(File.ReadAllText(sync.Replace(".arch", ".conf")).Split('"')[1]);

                    addOptimiseWatcher(dir);

                    string link = "https://www.youtube.com/playlist?" + Path.GetFileNameWithoutExtension(sync);

                    NYoutubeDL.YoutubeDL dl = new NYoutubeDL.YoutubeDL();
                    dl.YoutubeDlPath = AppContext.BaseDirectory + "ressources\\youtube-dl.exe";
                    dl.Options.GeneralOptions.ConfigLocation = sync.Replace(".arch", ".conf");
                    dl.VideoUrl = link;
                    dl.Options.VideoSelectionOptions.DownloadArchive = sync;
                    dl.Options.GeneralOptions.Update = true;
                    dl.StandardOutputEvent += Dl_StandardOutputEvent;
                    dl.StandardErrorEvent += Dl_StandardErrorEvent;

                    await dl.DownloadAsync();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + e.StackTrace, e.Source);
            }

            logs.Clear();
        }

        private readonly List<string> queue = new List<string>();

        private void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            queue.Add(e.Name);
        }

        private void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            if (queue.Contains(e.Name))
            {
                MessageBox.Show("moved!");
            }
        }

        public async void download(string video)
        {
            NYoutubeDL.YoutubeDL dl = new NYoutubeDL.YoutubeDL();
            dl.YoutubeDlPath = AppContext.BaseDirectory + "ressources\\youtube-dl.exe";
            dl.Options.PostProcessingOptions.ExtractAudio = true;
            dl.Options.PostProcessingOptions.AudioFormat = NYoutubeDL.Helpers.Enums.AudioFormat.mp3;
            dl.Options.PostProcessingOptions.AddMetadata = true;
            dl.Options.GeneralOptions.Update = true;
            dl.StandardOutputEvent += Dl_StandardOutputEvent;
            dl.StandardErrorEvent += Dl_StandardErrorEvent;
            await dl.DownloadAsync(video);
        }

        public void Dl_StandardErrorEvent(object sender, string e)
        {
            if (e.ToLower().Contains("debug"))
                return;

            MessageBox.Show(e, "syncer: error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private readonly List<string> logs = new List<string>();
        private readonly List<string> impLogs = new List<string>();
        private readonly List<VideoDownload> downloadedMusic = new List<VideoDownload>();

        public async void Dl_StandardOutputEvent(object sender, string e)
        {
            logs.Add(e);

            if (e.Contains("Downloading webpage") && e.Contains("[youtube]"))
            {
                string id = e.Split(':')[0].Split(' ')[1];
                impLogs.Add(id);
                Google.Apis.YouTube.v3.Data.Video vid = await youtube.byId(id);

                bool acrop = (vid != null && await youtube.isMusic(vid));
                bool split = await youtube.hasChapters(vid);

                downloadedMusic.Add(new VideoDownload(vid, vid.Snippet.Title + ".mp3", acrop, split));
            }
            if (e.Contains("[ffmpeg] Destination: "))
            {
                impLogs.Add(e.Split(":".ToCharArray(), count: 2)[1].Trim());
            }
        }

        private class VideoDownload
        {
            public Google.Apis.YouTube.v3.Data.Video video;
            public string title;
            public bool acrop = false;
            public bool splitChapters = false;

            public VideoDownload(Google.Apis.YouTube.v3.Data.Video vid, string videoTitle, bool autoCrop = false, bool splitByChapter = false)
            {
                video = vid;
                title = videoTitle;
                acrop = autoCrop;
                splitChapters = splitByChapter;
            }
        }
    }
}
