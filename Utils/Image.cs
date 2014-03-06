using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RX14.Utils
{
    /// <summary>
    /// Image Utils
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Returns a bitmap image from a filename
        /// </summary>
        /// <param name="filename">The filename of the image to load</param>
        /// <param name="UriKind">UriKind so we know if it's relative or absolute</param>
        /// <param name="silent">Whether to log messages</param>
        /// <param name="ignoreError">Whether to process or ignore errors</param>
        /// <param name="errorActions">Actions to pass to showError</param>
        /// <returns></returns>
        public static BitmapImage loadBitmapFromFile(string filename, UriKind UriKind = UriKind.RelativeOrAbsolute, bool silent = false, bool ignoreError = false, string[] errorActions = null)
        {
            //Encode BitmapImage
            BitmapImage UserHead = new BitmapImage();
            try
            {
                UserHead.BeginInit();
                UserHead.CacheOption = BitmapCacheOption.OnLoad;
                UserHead.UriSource = new Uri(filename, UriKind);
                UserHead.EndInit();

                if (!silent) Logging.logMessage("Successfully loaded image: " + filename);
                return UserHead;
            }
            catch
            {
                if (!ignoreError) Logging.showError("Failed to display image: " + filename, errorActions);
                return null;
            }
        }
    }
}
