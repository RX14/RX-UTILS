using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace Gusto.Launcher.Utils
{
    /// <summary>
    /// Class for handling file functions
    /// </summary>
    class Files
    {
        /// <summary>
        /// Deletes all files in an array
        /// </summary>
        /// <param name="files">Array of files to delete</param>
        public static void DeleteFileArray(string[] files)
        {
            foreach (string i in files)
            {
                try
                {
                    Logging.logMessage("Deleted \"" + i + '"', 1);
                    File.Delete(i);
                }
                catch (Exception e)
                { 
                    Logging.logMessage(e.ToString(), 3);
                }
            }
        }

        /// <summary>
        /// Writes text to a file
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="file">The filename to write to</param>
        /// <param name="append">Whether to append to the file or overwrite</param>
        public static void writeToFile(string text, string file, bool append = true)
        {
            //NO LOGGER in here because of recursion!
            try
            {
                StreamWriter sw = new StreamWriter(file, append);
                sw.WriteLine(text);
                sw.Close();
                sw = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void unZip(string FileName, string Directory, string FileFilter = "", bool abortLaunchOnExtractErrors = false, bool showErrors = true)
        {
            FastZip fz = new FastZip();
            try
            {
                fz.ExtractZip(FileName, Directory, FileFilter);
            }
            catch (Exception e)
            {
                if (showErrors) Logging.showError(e.ToString(), quitLaunch: abortLaunchOnExtractErrors);
            }
            fz = null;
        }

        public static string GetJavaInstallPath()
        {
            String javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";
            var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(javaKey);
            String currentVersion = baseKey.GetValue("CurrentVersion").ToString();
            var homeKey = baseKey.OpenSubKey(currentVersion);
            return homeKey.GetValue("JavaHome").ToString();       
        }
    }
}
