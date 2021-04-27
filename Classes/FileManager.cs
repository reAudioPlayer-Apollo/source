using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML
{
    public static class FileManager
    {
        static string supportedFormats = ".mp3|.wav|.flac";

        public static bool isSupported(string file)
        {
            return (supportedFormats.Split('|').Contains(Path.GetExtension(file)));
        }

        public static bool isTaggable(string file)
        {
            return false;
        }

        public static bool isVideo(string file)
        {
            return false;
        }
    }
}
