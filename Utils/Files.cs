using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace RX14.Utils
{
    /// <summary>
    /// Class for handling file functions
    /// </summary>
    public class Files
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
        public static bool writeToFile(string text, string file, bool append = true, bool ignoreError = false, string[] errorActions = null)
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
                if (!ignoreError) Logging.showError(e.ToString(), errorActions);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Unzips a file
        /// </summary>
        /// <param name="ZipFile">The Zip file to unpack</param>
        /// <param name="Directory">The directory to unpack to</param>
        /// <param name="FileFilter">#ZipLib-Style FileFilter</param>
        /// <param name="ErrorActions">The ErrorActions to pass to the logger</param>
        /// <param name="ignoreError">Whether to showError on failure</param>
        /// <param name="silent">Whether to show basic messages</param>
        public static bool unZip(string ZipFile, string Directory, string FileFilter = "", string[] ErrorActions = null, bool silent = false, bool ignoreError = false)
        {
            if (!silent) Logging.logMessage("Unzipping: " + ZipFile);
            FastZip fz = new FastZip();
            try
            {
                fz.ExtractZip(ZipFile, Directory, FileFilter);
            }
            catch (Exception e)
            {
                if (!ignoreError) Logging.showError(e.ToString(), ErrorActions);
                return false;
            }
            fz = null;
            return true;
        }

        /// <summary>
        /// Zips up a folder
        /// </summary>
        /// <param name="Directory">Directory to zip up</param>
        /// <param name="ZipFile">Zip file to create</param>
        /// <param name="FileFilter">#ZipLib FileFilter</param>
        /// <param name="silent">Whether to show basic messages</param>
        /// <param name="ignoreError">Whether to ignore Errors</param>
        /// <param name="errorActions">Actions to perform on error</param>
        public static bool Zip(string Directory, string ZipFile, string FileFilter = "", bool silent = false, bool ignoreError = false, string[] errorActions = null)
        {
            if (!silent) Logging.logMessage("Zipping: " + Directory);
            FastZip fz = new FastZip();
            try
            {
                fz.CreateZip(ZipFile, Directory, true, FileFilter);
            }
            catch (Exception e)
            {
                if (!ignoreError) Logging.showError("Failed to zip directory" + Environment.NewLine + e.ToString(), errorActions);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the Java install path
        /// </summary>
        /// <returns>the java install path</returns>
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
