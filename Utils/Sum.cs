using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace RX14.Utils
{
    /// <summary>
    /// Contains utils for summing files.
    /// </summary>
    public class Sum
    {
        /// <summary>
        /// Gives a SHA1 Sum of the specified file
        /// </summary>
        /// <param name="filelocation">Filename to sum</param>
        /// <returns>The SHA1 in string format</returns>
        public static string sha1sumFile(string filelocation)
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

        [Obsolete("Use sha1sumFile method")]
        public static string sha1sum(string filelocation)
        {
            return sha1sumFile(filelocation);
        }

        public static string sha1sumString(string str)
        {
            using (SHA1Managed _sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(_sha1.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty);
            }
        }
    }
}
