/*
 * not working yet, may be used in a future version
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using System.IO;

namespace reAudioPlayerML
{
    public class ImageToText
    {
        public ImageToText()
        {
            Bitmap bmap = new Bitmap("any file");
            int w = bmap.Width, h = bmap.Height;

            Bitmap bmapcrp = cropImage(bmap, new Rectangle(3 * w / 20, 2 * h / 3, 6 * w / 10, h / 6)) as Bitmap;
            bmapcrp.Save("example-crp.png");

            bmap.Dispose();

            var text = GetText(bmapcrp);
            bmapcrp.Dispose();
            Debug.WriteLine(text);
        }

        private Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public string GetText(Bitmap imgsource)
        {
            var ocrtext = string.Empty;
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                using (var img = PixConverter.ToPix(imgsource))
                {
                    using (var page = engine.Process(img))
                    {
                        ocrtext = page.GetText();
                    }
                }
            }

            return ocrtext;
        }
    }
}
