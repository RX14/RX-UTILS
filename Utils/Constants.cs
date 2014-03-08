using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RX14.Utils
{
    class Constants
    {
        /// <summary>
        /// https://minotar.net/helm/
        /// Base download url for downloading Helmets
        /// </summary>
        public const string HelmDownloadURL = "https://minotar.net/helm/";

        /// <summary>
        /// https://s3.amazonaws.com/Minecraft.Download/
        /// Base MinecraftDownload URL
        /// </summary>
        public const string MinecraftDownload = "https://s3.amazonaws.com/Minecraft.Download/";

        /// <summary>
        /// https://s3.amazonaws.com/Minecraft.Download/versions/
        /// Base Minecraft Versions API URL
        /// </summary>
        public const string MinecraftVersionsBase = MinecraftDownload + "versions/";
    }
}
