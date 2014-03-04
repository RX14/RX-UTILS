using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gusto.Launcher.Utils
{
    /// <summary>
    /// Handles Logging
    /// </summary>
    class Logging
    {
        /// <summary>
        /// Logs a Message
        /// 
        /// LogLevels:
        /// 1 = Debug,
        /// 2 = Info,
        /// 3 = Warn,
        /// 4 = Error,
        /// </summary>
        /// <param name="logMessage">Message to log</param>
        /// <param name="logLevel">Error level of the log</param>
        public static void logMessage(string logMessage, int logLevel = 2)
        {
            //Generate string based on logLevel
            string logLevelString;
            if (logLevel == 1) logLevelString = "DEBUG";
            else if (logLevel == 2) logLevelString = "INFO";
            else if (logLevel == 3) logLevelString = "WARN";
            else if (logLevel == 4) logLevelString = "ERROR";
            else logLevelString = "Info";
            
            //Generate Log String
            string toLog = DateTime.Now.ToString("hh:mm:ss tt") + " [GustoLauncher] ";
            toLog += "[" + logLevelString + "] ";
            toLog += logMessage;

            //log to Console
            Console.WriteLine(toLog);

            //Log to Console Window
            CurrentStateUtils.CurrentState.ConsoleBuffer.Add(toLog);

            //Log to File
            Files.writeToFile(toLog, "launcher.log");
        }

        /// <summary>
        /// Sows a dialog for an error
        /// </summary>the string to show & log</param>
        public static void showError(string logMessage, bool quitApplication = false, bool quitLaunch = false)
        {
            Logging.logMessage(logMessage, 4);
            MessageBox.Show(logMessage, "ERROR!");
            if (quitApplication)
            {
                Environment.Exit(0);
            }

            if (quitLaunch)
            {
                Minecraft.Launch.abortLaunch();
            }
        }
    }
}
