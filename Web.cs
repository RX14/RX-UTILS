﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Gusto.Launcher.Utils
{
    /// <summary>
    /// Class for handling web functions
    /// </summary>
    class Web
    {
        /// <summary>
        /// Downloads a file from the internet to a directory.
        /// </summary>
        /// <param name="URL">The URL of the file to download.</param>
        /// <param name="downloadDirectory">The directory to download the file to.</param>
        /// <param name="overwrite">Whether to overwrite what's there</param>
        public static bool downloadFile(string URL, string downloadDirectory, bool overwrite = false, bool showErrors = true, bool specifyDownloadFile = false, bool errorOnMainFail = true, bool abortLaunchOnFail = false)
        {

            //Get filename from URL
            string filename = URL.Split('/')[URL.Split('/').Length - 1];
            string fn = downloadDirectory.TrimEnd('/').Split('/')[downloadDirectory.TrimEnd('/').Split('/').Length - 1];
            string dir = downloadDirectory.Remove(downloadDirectory.Length - fn.Length);

            //If the file exists check if overwrite is accepted
            if (!(File.Exists(downloadDirectory + "/" + filename) | File.Exists(downloadDirectory)) || overwrite)
            {
                if (!specifyDownloadFile)
                {
                    //If the directory doesn't exist, create it
                    if (!Directory.Exists(downloadDirectory))
                    {
                        Logging.logMessage("Created directory " + downloadDirectory, 2);
                        Directory.CreateDirectory(downloadDirectory);
                    }
                }
                else
                {
                    //If the directory doesn't exist, create it
                    if (!Directory.Exists(dir))
                    {
                        Logging.logMessage("Created directory " + dir, 2);
                        Directory.CreateDirectory(dir);
                    }
                }

                //Acctually download file
                try
                {
                    if (showErrors) Logging.logMessage("Trying to download " + URL + " to " + downloadDirectory, 2);
                    WebClient wc = new WebClient();
                    if (specifyDownloadFile == true)
                    {
                        wc.DownloadFile(new Uri(URL), downloadDirectory);
                    }
                    else
                    {
                        wc.DownloadFile(new Uri(URL), downloadDirectory + "/" + filename);
                    }
                    wc.Dispose();
                    wc = null;
                }
                catch (Exception e)
                {
                    if (errorOnMainFail) Logging.showError("Failed to download " + URL + " :" + e.ToString(), false, abortLaunchOnFail);
                    return false;
                }
            } else {
                if (showErrors) Logging.logMessage("Didn't download " + URL + " to " + downloadDirectory + " because it already existed", 2);
            }
            return true;
        }

        /// <summary>
        /// Gets the string returned from given URL
        /// </summary>
        /// <param name="URL">The URL to get</param>
        /// <returns>String fom thr URL</returns>
        public static string stringFromURL(string URL)
        {
            try
            {
                WebClient wc = new WebClient();
                byte[] raw = wc.DownloadData(URL);
                return System.Text.Encoding.UTF8.GetString(raw);
            }
            catch (Exception e)
            {
                Logging.logMessage("Failed to get string from " + URL + " :" + e.ToString(), 3);
                return null;
            }
        }

        /// <summary>
        /// POST's an URL with some data
        /// </summary>
        /// <param name="URL">URL to Post</param>
        /// <param name="data">Data to POST with</param>
        /// <param name="contentType">Content Type to set on the Request</param>
        /// <returns>Returned String</returns>
        public static string POST_URL(string URL, string data, string contentType = "text/plain")
        {
            //Create WebRequest with correct Headers and Method
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(URL);
            //wr.Accept = "application/json";
            wr.ContentType = contentType;
            wr.Method = "POST";

            //Convert data into ByteArray
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);

            //string test = System.Text.Encoding.UTF8.GetString(bytes);

            //Send Data
            Stream send = wr.GetRequestStream();
            send.Write(bytes, 0, bytes.Length);
            send.Close();

            WebResponse response = null;
            try
            {
                //Try to get response
                response = wr.GetResponse();
            }
            catch (WebException ex)
            {
                //On error get error response
                response = ex.Response;
            }

            //Parse Response
            Stream responsestream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responsestream);
            string content = sr.ReadToEnd();

            return content;
        }
    }
}
