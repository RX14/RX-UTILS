using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Net;
using System.IO;
//using System.Drawing;


namespace Gusto.Launcher.Utils
{
    /// <summary>
    /// Common functions for the Launcher
    /// </summary>
    class Skin
    {
        /// <summary>
        /// Gets a BitmapImage of a head from a Minecraft Username
        /// </summary>
        /// <param name="username">The Minecraft Username</param>
        /// <param name="imgSize">The side length of the image to return.</param>
        /// <returns>Returns a BitmapImage</returns>
        public static BitmapImage getUserHead(string username, int imgSize = 0)
        {
            Logging.logMessage("Trying to display head for " + username + "...");

            string minotarLocation;

            //Change download URL depending on whether imgSize is specified
            if (imgSize == 0)
            {
                minotarLocation = Constants.HelmDownloadURL + username + ".png";
            } else {
                minotarLocation = Constants.HelmDownloadURL + username + "/" + imgSize + ".png";
            }

            Web.downloadFile(minotarLocation, "cache/UserHeads/" + username);

            var uri = new System.Uri(Directory.GetCurrentDirectory() + "/cache/UserHeads/" + username + "/" + minotarLocation.Split('/')[minotarLocation.Split('/').Length - 1]);
            var converted = uri.AbsoluteUri;

            //Encode BitmapImage
            BitmapImage UserHead = new BitmapImage();
            try
            {
                UserHead.BeginInit();
                UserHead.CacheOption = BitmapCacheOption.OnLoad;
                UserHead.UriSource = uri;
                UserHead.EndInit();

                Logging.logMessage("Successfully downloaded head");
                return UserHead;
            } catch {
                Gusto.Launcher.Properties.Settings.Default.ToDelete += uri.LocalPath + "|";
                return null;
            }
        }
    }
}
