﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.FtpClient;

namespace RX14.Utils
{
    /// <summary>
    /// Class for handling web functions
    /// </summary>
    public class HTTP
    {
        /// <summary>
        /// Downloads a file from the internet to a directory Asynchronously.
        /// </summary>
        /// <param name="URL">The URL of the file to download.</param>
        /// <param name="downloadDirectory">The directory to download the file to.</param>
        /// <param name="overwrite">Whether to overwrite what's there</param>
        /// <param name="errorActions">Actions to pass to showError on main error.</param>
        /// <param name="silent">Whether to show messages when it starts downloading or if the file allready existed</param>
        /// <param name="ignoreError">Whether to error if the main try loop fails.</param>
        /// <param name="specifyDownloadFile">Whether the downloadDirectory includes the file name to download to</param>
        public static async Task<bool> downloadFileAsync(string URL, string downloadDirectory, bool overwrite = false, bool silent = false, bool specifyDownloadFile = false, bool ignoreError = false, string[] errorActions = null)
        {

            //Get filename from URL
            string filename = Path.GetFileName(new Uri(URL).AbsolutePath);
            string dir = Path.GetDirectoryName(downloadDirectory);

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
                    if (!silent) Logging.logMessage("Trying to download " + URL + " to " + downloadDirectory, 2);
                    WebClient wc = new WebClient();
                    if (specifyDownloadFile)
                    {
                        await wc.DownloadFileTaskAsync(new Uri(URL), downloadDirectory);
                    }
                    else
                    {
                        await wc.DownloadFileTaskAsync(new Uri(URL), downloadDirectory + "/" + filename);
                    }
                    wc.Dispose();
                    wc = null;
                }
                catch (Exception e)
                {
                    if (!ignoreError) Logging.showError("Failed to download " + URL + " :" + e.ToString(), errorActions);
                    return false;
                }
            } else {
                if (!silent) Logging.logMessage("Didn't download " + URL + " to " + downloadDirectory + " because it already existed", 2);
            }
            return true;
        }

        public static bool downloadFile(string URL, string downloadDirectory, bool overwrite = false, bool silent = false, bool specifyDownloadFile = false, bool ignoreError = false, string[] errorActions = null)
        {
            return downloadFileAsync(URL, downloadDirectory, overwrite, silent, specifyDownloadFile, ignoreError, errorActions).Result;
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
        /// POSTs an URL with some data
        /// </summary>
        /// <param name="URL">URL to Post</param>
        /// <param name="data">Data to POST with</param>
        /// <param name="contentType">Content Type to set on the Request</param>
        /// <returns>What the webpage returns</returns>
        public static string POST_URL(string URL, string data, string contentType = "text/plain", bool ignoreError = false, string[] errorActions = null)
        {
            try
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
            catch (Exception e)
            {
                if (!ignoreError) Logging.showError("Failed to POST URL:" + Environment.NewLine + e.ToString(), errorActions);
                return null;
            }
        }
    }

    /// <summary>
    /// Handles FTP thingies
    /// </summary>
    public class FTP
    {
        /// <summary>
        /// Uploads a file to FTP
        /// </summary>
        /// <param name="localFileName">Local file to upload</param>
        /// <param name="uploadPath">URI of the file to upload</param>
        /// <param name="silent"></param>
        /// <param name="ignoreError"></param>
        /// <param name="errorActions"></param>
        public static void uploadFTP(string Host, string localFileName, string serverPath, NetworkCredential Credentials, bool silent = false, bool ignoreError = false, string[] errorActions = null)
        {
            try
            {
                int bufferSize = 8192;
                FtpClient FTP = new FtpClient();
                FTP.ValidateCertificate += FTPClient_ValidateCertificate;
                FTP.Host = Host;
                FTP.Credentials = Credentials;
                FTP.Connect();

                if (!silent) Logging.logMessage("Connected to: " + Host, 1);

                if (!FTP.DirectoryExists(serverPath))
                {
                    Logging.logMessage("Created directory on server: " + serverPath);
                    FTP.CreateDirectory(serverPath);
                }

                FileStream localFile = new FileStream(localFileName, FileMode.OpenOrCreate);
                byte[] Buffer = new byte[bufferSize];
                Stream writeStream = FTP.OpenWrite(serverPath + Path.GetFileName(localFileName), FtpDataType.Binary);
                int bytesSent = localFile.Read(Buffer, 0, bufferSize);

                while (bytesSent != 0)
                {
                    writeStream.Write(Buffer, 0, bytesSent);
                    bytesSent = localFile.Read(Buffer, 0, bufferSize);
                }

                localFile.Close();
                writeStream.Close();
                FTP.Dispose();

                if (!silent) Logging.logMessage("Disconnedted from FTP server", 1);

            }
            catch (Exception e)
            {
                if (!ignoreError) Logging.showError("Failed to upload file via FTP:" + Environment.NewLine + e.ToString(), errorActions);
            }

        }

        static void FTPClient_ValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
        {
            e.Accept = true;
        }
    }
}
