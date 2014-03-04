using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RX14.Utils
{
    /// <summary>
    /// Handles Logging
    /// </summary>
    public class Logging
    {
        public delegate void _showErrorDelegate(string logMessage, string[] actions);
        public delegate void _customLogDelegate(string logMessage, int logLevel);
        public static List<_showErrorDelegate> showErrorDelegates = new List<_showErrorDelegate>();
        public static List<_customLogDelegate> customLogDelegates = new List<_customLogDelegate>();
        public static string LogPrefix;
        
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
            else logLevelString = "LOG-LVL-ERR";
            
            //Generate Log String
            string toLog = DateTime.Now.ToString("hh:mm:ss tt") + " ["+ LogPrefix + "] ";
            toLog += "[" + logLevelString + "] ";
            toLog += logMessage;

            //log to Console
            Console.WriteLine(toLog);

            //Log to Custom Delegates
            foreach (_customLogDelegate customLogDelegate in customLogDelegates)
            {
                customLogDelegate.Invoke(toLog, logLevel);
            }
        }

        public static void showError(string logMessage, string[] actions = null)
        {
            foreach (_showErrorDelegate showErrorDelegate in showErrorDelegates)
            {
                showErrorDelegate.Invoke(logMessage, actions);
            }
        }
    }
}
