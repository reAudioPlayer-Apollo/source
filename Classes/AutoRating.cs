using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML
{
    public class AutoRating
    {
        public Stats stats = new Stats();

        public int score
        {
            get
            {
                int _score;
                var playScore = stats.dailyPlayScore * 2 + stats.weeklyPlayScore / 3;
                _score = playScore + stats.boost;
                return _score > 100 ? 100 : _score;
            }
        }

        public class Stats
        {
            internal static int maxDailyPlayCount = 0;
            internal static int maxWeeklyPlayCount = 0;

            public static int calculatePlayExperience(int percentage)
            {
                if (percentage >= 98)
                {
                    return percentage;
                }

                double y = (2.295238 * percentage)
                    - (0.03428571 * percentage * percentage)
                    + (0002133333 * percentage * percentage);

                return (int)Math.Round(y);
            }

            public int dailyPlayScore
            {
                get
                {
                    return 100 * dailyPlayCount / maxDailyPlayCount;
                }
            }

            public int weeklyPlayScore
            {
                get
                {
                    return 100 * weeklyPlayCount / maxWeeklyPlayCount;
                }
            }

            private int dailyPlayCount
            {
                get
                {
                    var maxAge = DateTime.Now.AddDays(-1);
                    var t = plays.Where(x => x < maxAge).Count();
                    maxDailyPlayCount = Math.Max(t, maxDailyPlayCount);
                    return t;
                }
            }

            private int weeklyPlayCount
            {
                get
                {
                    var maxAge = DateTime.Now.AddDays(-7);
                    var t = plays.Where(x => x < maxAge).Count();
                    maxWeeklyPlayCount = Math.Max(t, maxWeeklyPlayCount);
                    return t;
                }
            }

            public int boost = 0;

            public List<DateTime> plays;
        }
    }
}
