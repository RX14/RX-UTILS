using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Net;
using System.IO;
using RX14.Utils;


namespace RX14.Utils.Minecraft
{
    /// <summary>
    /// Class for handling skins
    /// </summary>
    public class Skin
    {
        /// <summary>
        /// Gets a BitmapImage of a head from a Minecraft Username
        /// </summary>
        /// <param name="username">The Minecraft Username</param>
        /// <param name="imgSize">The side length of the image to return.</param>
        /// <returns>Returns a BitmapImage</returns>
        public static BitmapImage getUserHead(string username, int imgSize = -1, bool silent = false, string[] errorAction = null)
        {
            if (!silent) Logging.logMessage("Trying to display head for " + username + "...");

            string minotarLocation;

            //Change download URL depending on whether imgSize is specified
            if (imgSize == -1)
            {
                minotarLocation = Constants.HelmDownloadURL + username + ".png";
            } else {
                minotarLocation = Constants.HelmDownloadURL + username + "/" + imgSize + ".png";
            }

            HTTP.downloadFile(minotarLocation, "cache/UserHeads/" + username, silent: true);

            return Image.loadBitmapFromFile("cache/UserHeads/" + username + "/" + minotarLocation.Split('/')[minotarLocation.Split('/').Length - 1], UriKind.Relative);
        }
    }
}
