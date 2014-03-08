using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RX14.Utils.Types.Minecraft.MCVersionList
{
    public class Latest
    {
        public string snapshot { get; set; }
        public string release { get; set; }
    }

    public class Version
    {
        public string id { get; set; }
        public string time { get; set; }
        public string releaseTime { get; set; }
        public string type { get; set; }
    }

    public class Root
    {
        public Latest latest { get; set; }
        public List<Version> versions { get; set; }
    }
}
