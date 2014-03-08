using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RX14.Utils;

namespace RX14.Utils.Minecraft
{
    public class VersionList
    {
        public static Types.Minecraft.MCVersionList.Root GetVersionList()
        {
            try
            {
                HTTP.downloadFile(Constants.MinecraftVersionsBase + "versions.json", "cache/versions/");
                return Json.DeserializeFile<Types.Minecraft.MCVersionList.Root>("cache/versions/versions.json", ignoreError: true);
            }
            catch (Exception e)
            {
                Logging.showError("Error parsing version JSON: " + e.ToString());
                return null;
            }
        }

        public static List<String> ParseVersionList(string[] typefilters)
        {
            List<String> Versions = new List<string>();

            var VersionList = GetVersionList();

            if (VersionList == null)
            {
                return null;
            }

            foreach (Types.Minecraft.MCVersionList.Version Version in GetVersionList().versions)
            {
                foreach (string filter in typefilters)
                {
                    if (Version.type == filter)
                    {
                        Versions.Add(Version.id);
                        break;
                    }
                }
            }

            return Versions;
        }
    }
}
