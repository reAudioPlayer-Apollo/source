using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML.HttpServer.API
{
    class Static
    {
        public static string GetStream(Image image)
        {
            if (image is null)
                return "";

            try
            {
                Image i = image.Clone() as Image;

                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, i.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
            catch
            {
                return "";
            }
        }

        public static string GetStream(string path)
        {
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        public static async Task SendStringAsync(IHttpContext context, string content,
            string contentType = "text/html", Encoding encoding = null)
        {
            encoding = encoding is null ? Encoding.Latin1 : encoding;

            await context.SendStringAsync(content, contentType, encoding);
        }
    }

    class GeneralAPI: WebApiController
    {
        [Route(HttpVerbs.Get, "/version")]
        public async Task getVersion(string user)
        {
            await Static.SendStringAsync(HttpContext, "reAudioPlayer Apollo .5");
        }
    }
}
