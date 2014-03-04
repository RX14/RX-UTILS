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
        /// <summary>
        /// Delegate for handling showError
        /// </summary>
        /// <param name="errorMessage">The error message to send to the handler</param>
        /// <param name="actions">List of actions to pass to the handler</param>
        public delegate void _showErrorHandler(string errorMessage, string[] actions);

        /// <summary>
        /// Delegate for handling Log Messages
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="logLevel"></param>
        public delegate void _customLogHandler(string logMessage, int logLevel);
        public static List<_showErrorHandler> showErrorHandlers = new List<_showErrorHandler>();
        public static List<_customLogHandler> customLoggers = new List<_customLogHandler>();
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
            foreach (_customLogHandler customLogDelegate in customLoggers)
            {
                customLogDelegate.Invoke(toLog, logLevel);
            }
        }

        public static void showError(string errorMessage, string[] actions = null)
        {
            logMessage(errorMessage, 4);
            foreach (_showErrorHandler showErrorDelegate in showErrorHandlers)
            {
                showErrorDelegate.Invoke(errorMessage, actions);
            }
        }
    }
}
