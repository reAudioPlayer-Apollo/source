using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML
{
    public static class QRCodeForWebClient
    {
        public static string GetPhysicalIPAdress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                if (addr != null && !addr.Address.ToString().Equals("0.0.0.0"))
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return String.Empty;
        }

        public static Image getImage()
        {
            string link = $"http://{GetPhysicalIPAdress()}:8080/";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            var qrCode = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            var qrcode = new QRCode (qrCode);
            return qrcode.GetGraphic(20, "#333333", "#f5f5f5", drawQuietZones: false);
        }
    }
}
