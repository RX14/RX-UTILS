using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Gusto.Launcher.Utils
{
    class Sum
    {
        public static string sha1sum(string filelocation)
        {
            FileStream fs = new FileStream(filelocation, FileMode.Open);
            BufferedStream bs = new BufferedStream(fs);
            SHA1Managed _sha1 = new SHA1Managed();

            byte[] hash = _sha1.ComputeHash(bs);
            StringBuilder sha1 = new StringBuilder(2 * hash.Length);
            foreach (byte b in hash)
            {
                sha1.AppendFormat("{0:X2}", b);
            }

            fs.Close();

            return sha1.ToString();
        }
    }
}
