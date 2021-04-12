using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML
{
    public class OptimizeDL
    {
        public string[] supportedFormats = new string[]
        {
            "mp3",
            //"wav",
            //"mp4",
            //"flac"
        };

        public int acrop(FileInfo file)
        {
            int height, width, x_offset, y_offset, width_new;

            // 16 : 9

            // THIS VALS SHOULD BE SET
            height = 720;
            width = 1280;

            // THIS VALS SHOULD NOT BE SET
            width_new = height;
            y_offset = 0;
            x_offset = (width - height) / 2;


            List<FileInfo> todo = new List<FileInfo>();

            foreach (var supportedFormat in supportedFormats)
            {
                if (file.FullName.Contains($".{supportedFormat}"))
                    todo.Add(file);
            }
            
            if (todo.Count == 0)
                return (-20);

            if (!File.Exists(AppContext.BaseDirectory + "ressources\ffmpeg.exe"))
                return -30;

            string workingPictureFormat = "jpg";

            var t = file.FullName.Split('.');

            var baseFile = file.FullName.Remove(file.FullName.Length - (t[t.Length - 1].Length + 1));

            /* extract thumbnail */

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = AppContext.BaseDirectory + "ressources\ffmpeg.exe";
            string name = Path.GetFileNameWithoutExtension(startInfo.FileName);

            startInfo.Arguments = $"-y -i \"{file.FullName}\" \"{baseFile}.old.{workingPictureFormat}\"";

            var test = Process.Start(startInfo);

            while (!test.HasExited) ;

            /* crop thumbnail */
            startInfo.Arguments = $"-i \"{baseFile}.old.{workingPictureFormat}\" -filter:v \"crop={width_new}:{height}:{x_offset}:{y_offset}\" \"{baseFile}.new.{workingPictureFormat}\"";

            test = Process.Start(startInfo);

            while (!test.HasExited) ;

            /* merge */
            startInfo.Arguments = $"-i \"{file.FullName}\" -i \"{baseFile}.new.{workingPictureFormat}\" -map 0:0 -map 1:0 -c copy -id3v2_version 3 -metadata:s:v title=\"Album cover\" -metadata:s:v comment=\"Cover (front)\" \"{baseFile}.temp.mp3\"";

            test = Process.Start(startInfo);

            while (!test.HasExited) ;

            File.Delete($"{baseFile}.old.{workingPictureFormat}");
            File.Delete($"{baseFile}.new.{workingPictureFormat}");

            if (File.Exists($"{baseFile}.temp.mp3"))
            {
                File.Copy($"{baseFile}.temp.mp3", $"{baseFile}.mp3", true);
                File.Delete($"{baseFile}.temp.mp3");
            }

            return 0;
        }

        public int splitByChapter(FileInfo file, Google.Apis.YouTube.v3.Data.Video video)
        {
            Search.Youtube yt = new Search.Youtube();
            List<string> chapters = yt.getChapters(video).Result;
            var stamps = yt.chapterStamps;

            var outputFolder = Path.GetDirectoryName(file.FullName) + $"\\{Path.GetFileNameWithoutExtension(file.FullName)}";

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            for (int i = 1; i <= stamps.Count; i++)
            {
                string outputFile = outputFolder + $"\\{i} - {chapters[i - 1].Replace(chapters[i - 1].Split(' ')[0], "").Replace(".", "-").Replace(":", "").Trim()}.mp3";
                outputFile = outputFile.Replace("- - ", "- ");

                if (i == stamps.Count)
                {
                    try
                    {
                        split(file.FullName, stamps[i - 1], outputFile);
                    }
                    catch { }
                }
                else
                {
                    split(file.FullName, stamps[i - 1], stamps[i], outputFile);
                }
            }


            return 0;
        }

        private void split(string input, TimeSpan start, TimeSpan end, string output)
        {
            if (!File.Exists(AppContext.BaseDirectory + "ressources\ffmpeg.exe"))
                return;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = AppContext.BaseDirectory + "ressources\ffmpeg.exe";
            string name = Path.GetFileNameWithoutExtension(startInfo.FileName);

            input = input.Replace("\\", "/");
            output = output.Replace("\\", "/");

            startInfo.Arguments = $"-y -i \"{input}\" -ss {start.TotalSeconds} -to {end.TotalSeconds} \"{output}\"";

            var test = Process.Start(startInfo);

            while (!test.HasExited) ;
        }

        private void split(string input, TimeSpan start, string output)
        {
            if (!File.Exists(AppContext.BaseDirectory + "ressources\ffmpeg.exe"))
                return;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = AppContext.BaseDirectory + "ressources\ffmpeg.exe";
            string name = Path.GetFileNameWithoutExtension(startInfo.FileName);

            input = input.Replace("\\", "/");
            output = output.Replace("\\", "/");

            startInfo.Arguments = $"-y -i \"{input}\" -ss {start.TotalSeconds} \"{output}\"";

            var test = Process.Start(startInfo);

            while (!test.HasExited) ;
        }
    }
}
