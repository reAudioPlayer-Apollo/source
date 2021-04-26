using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
                    image.Save(m, ImageFormat.Bmp);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
            catch (Exception e)
            {
                return e.Message;
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
        public async Task RGetVersion(string user)
        {
            await Static.SendStringAsync(HttpContext, version());
        }

        public string version()
        {
            return "reAudioPlayer Apollo .5";
        }

        public void handleWebsocket(ref Modules.WebSocket.MessageObject msg)
        {
            switch (msg.endpoint)
            {
                case "version":
                    msg.data = version();
                    break;

                default:
                    msg.data = "404";
                    break;
            }
        }
    }
}
