using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML
{
    public class AutoRating
    {
        public Stats stats;
        public string id;

        public AutoRating(MediaPlayer mediaPlayer, System.Windows.Media.MediaPlayer duration, string id)
        {
            this.id = id;
            stats = new Stats(mediaPlayer, duration, id);
        }

        public int score
        {
            get
            {
                int _score;
                var playScore = (stats.dailyPlayScore * 2 + stats.weeklyPlayScore) / 3;
                _score = playScore + stats.boost;
                return _score > 100 ? 100 : _score;
            }
        }

        public class Stats
        {
            internal static double maxDailyPlayCount = 0;
            internal static double maxWeeklyPlayCount = 0;
            MediaPlayer player;
            Stopwatch sw = new Stopwatch();
            TimeSpan duration
            {
                get
                {
                    return rPlayer.Dispatcher.Invoke(() => rPlayer is not null && rPlayer.NaturalDuration.HasTimeSpan ? rPlayer.NaturalDuration.TimeSpan : new TimeSpan());;
                }
            }
            public bool active = false;
            private string id;
            System.Windows.Media.MediaPlayer rPlayer;

            public static Dictionary<string, Dictionary<DateTime, int>> PlayCountCache
            {
                get
                {
                    var t = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<DateTime, int>>>(Properties.Settings.Default.autoRatingPlayCache);
                    return t is null ? new Dictionary<string, Dictionary<DateTime, int>>() : t;
                }
                set
                {
                    Properties.Settings.Default.autoRatingPlayCache = JsonConvert.SerializeObject(value);
                    Properties.Settings.Default.Save();
                }
            }


            public Stats(MediaPlayer mediaPlayer, System.Windows.Media.MediaPlayer rPlayer, string id)
            {
                if (mediaPlayer is not null)
                {
                    player = mediaPlayer;
                    player.Skip += Player_Skip;
                    player.Pause += Player_Pause;
                    player.Play += Player_Play;
                }
                this.rPlayer = rPlayer;
                this.id = id;

                var playCountCache = PlayCountCache;
                plays = playCountCache.ContainsKey(id) ? playCountCache[id] : new Dictionary<DateTime, int>();
            }

            private void updateCache()
            {
                var playCountCache = PlayCountCache;
                if (playCountCache.ContainsKey(id))
                {
                    playCountCache[id] = plays;
                }
                else
                {
                    playCountCache.Add(id, plays);
                }
                PlayCountCache = playCountCache;
            }

            private void Player_Play(object sender, EventArgs e)
            {
                if (active)
                    sw.Start();
            }

            private void Player_Pause(object sender, EventArgs e)
            {
                if (active)
                    sw.Stop();
            }

            private void Player_Skip(object sender, bool e)
            {
                if (active)
                {
                    active = false;
                    sw.Stop();
                    var t = sw.Elapsed;
                    if (duration.TotalSeconds != 0)
                    {
                        var time = t.TotalSeconds * 100 / duration.TotalSeconds;
                        addPlay(Convert.ToInt32(time));
                    }
                    sw.Reset();
                }
            }

            public void addPlay(int percentage)
            {
                int xp = CalculatePlayExperience(percentage);
                plays.Add(DateTime.Now, xp);
                updateCache();
            }

            private static int CalculatePlayExperience(int percentage)
            {
                if (percentage >= 98)
                {
                    return percentage;
                }

                double y = (2.295238 * percentage)
                    - (0.03428571 * percentage * percentage)
                    + (0.002133333 * percentage * percentage);

                return (int)Math.Round(y);
            }

            public int dailyPlayScore
            {
                get
                {
                    return (int)(maxDailyPlayCount == dailyPlayCount * 0 ? 0 : 100 * dailyPlayCount / maxDailyPlayCount);
                }
            }

            public int weeklyPlayScore
            {
                get
                {
                    return (int)(maxWeeklyPlayCount == weeklyPlayCount * 0 ? 0 : 100 * weeklyPlayCount / maxWeeklyPlayCount);
                }
            }

            private double dailyPlayCount
            {
                get
                {
                    var maxAge = DateTime.Now.AddDays(-1);
                    double t = plays.Where(x => x.Key > maxAge).Select(x => x.Value).ToArray().Sum();
                    t /= 100;
                    maxDailyPlayCount = Math.Max(t, maxDailyPlayCount);
                    return t;
                }
            }

            private double weeklyPlayCount
            {
                get
                {
                    var maxAge = DateTime.Now.AddDays(-7);
                    var t = plays.Select(x => x.Key).Where(x => x > maxAge).Count();
                    maxWeeklyPlayCount = Math.Max(t, maxWeeklyPlayCount);
                    return t;
                }
            }

            public int boost = 0;

            public Dictionary<DateTime, int> plays;
        }
    }
}
