/*
 * from https://github.com/dxstiny/cs-imageTools
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentColour
{
    class Finder
    {
        public Bitmap getColourAsShadowBitmap(Color colour)
        {
            int w = 170, h = 100;
            Bitmap pic = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int i = 0; i < w / 2; i++)
            {
                colour = Color.FromArgb(getAlpha(w, i), colour);

                for (int j = 0; j < h; j++)
                {
                    pic.SetPixel(i, j, colour);
                    pic.SetPixel(w - i - 1, j, colour);
                }
            }

            return pic;
        }

        private int getAlpha(int w, int i)
        {
            w /= 2;
            i /= 2;

            int value;

            if (i < w / 2)
                value = i;
            else
                value = w - i;

            var ret = Convert.ToInt32( Math.Round( value * 250.0 / w ) );
            return ret;
        }

        public Bitmap getColourAsBitmap(Color colour)
        {
            int w = 100, h = 100;
            Bitmap pic = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    pic.SetPixel(i, j, colour);
                }
            }

            return pic;
        }

        public List<int> vals = new List<int>();

        public List<int> sortList(ref List<Color> colours, ref List<int> appearances)
        {
            var colourWeight = 5;
            var appearanceWeight = 1;
            var maxScoreEach = 510;
            var weight = colourWeight + appearanceWeight;
            var maxScore = maxScoreEach * weight;

            cols = colours;

            var appearanceMultiplier = 510.0 / appearances[0];

            for (int i = 0; i < colours.Count; i++)
            {
                var colourScore = getDif(Convert.ToInt32(colours[i].R), Convert.ToInt32(colours[i].G)) + getDif(Convert.ToInt32(colours[i].R), Convert.ToInt32(colours[i].B));
                colourScore *= colourWeight;
                var appearanceScore = appearances[i] * appearanceMultiplier;
                appearanceScore *= appearanceWeight;
                var score = maxScore - (colourScore + Convert.ToInt32(appearanceScore));

                vals.Add(score);
            }

            var copy = new List<int>(vals);
            copy.Sort();

            var indices = new List<int>();

            for (int j = 0; j < 10 && j < copy.Count; j++)
            {
                indices.Add(vals.IndexOf(copy[j]));
            }

            return indices;
        }

        public List<Color> cols;

        int getDif(int v1, int v2)
        {
            if (v1 > v2)
                return (v1 - v2);
            else
                return (v2 - v1);
        }
    }

    class PictureAnalyser
    {
        public List<Color> TenMostUsedColors { get; private set; }
        public List<int> TenMostUsedColorIncidences { get; private set; }

        public Color MostUsedColor { get; private set; }
        public int MostUsedColorIncidence { get; private set; }

        private int pixelColor;

        private Dictionary<int, int> dctColorIncidence;

        // higher gap -> worse result, but faster
        public async Task GetMostUsedColor(Bitmap theBitMap, int gap = 1)
        {
            TenMostUsedColors = new List<Color>();
            TenMostUsedColorIncidences = new List<int>();

            MostUsedColor = Color.Empty;
            MostUsedColorIncidence = 0;

            dctColorIncidence = new Dictionary<int, int>();

            bool similar;

            var sizeMultiplier = (theBitMap.Size.Width / 1000);
            sizeMultiplier = sizeMultiplier >= 1 ? sizeMultiplier : 1;

            theBitMap.Save("getMostUsedColor.png");

            gap *= sizeMultiplier;

            for (int row = 0; row < theBitMap.Size.Width - (gap - 1); row += gap)
            {
                for (int col = 0; col < theBitMap.Size.Height - (gap - 1); col += gap)
                {
                    similar = false;

                    pixelColor = theBitMap.GetPixel(row, col).ToArgb();

                    if (dctColorIncidence.Keys.Contains(pixelColor))
                    {
                        dctColorIncidence[pixelColor]++;
                    }
                    else
                    {
                        foreach (KeyValuePair<int, int> pair in dctColorIncidence)
                        {
                            if (getColourDif(pair.Key, pixelColor) < 100000)
                            {
                                dctColorIncidence[pair.Key]++;
                                similar = true;
                                break;
                            }
                        }

                        if (!similar)
                        {
                            dctColorIncidence.Add(pixelColor, 1);
                        }
                    }
                }
            }

            var dctSortedByValueHighToLow = dctColorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            foreach (KeyValuePair<int, int> kvp in dctSortedByValueHighToLow)
            {
                TenMostUsedColors.Add(Color.FromArgb(kvp.Key));
                TenMostUsedColorIncidences.Add(kvp.Value);
            }

            MostUsedColor = Color.FromArgb(dctSortedByValueHighToLow.First().Key);
            MostUsedColorIncidence = dctSortedByValueHighToLow.First().Value;
        }

        double getColourDif(Color c1, Color c2)
        {
            var grC1 = .11 * c1.B + .59 * c1.G + .30 * c1.R;
            var grC2 = .11 * c2.B + .59 * c2.G + .30 * c2.R;

            var difference = (grC1 - grC2) * 100.0 / 255.0;

            return difference;
        }

        int getColourDif(int c1, int c2)
        {
            var difference = c1 - c2;

            if (difference < 0)
                difference *= -1;

            return difference;
        }
    }
}
