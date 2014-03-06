using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace RX14.Utils
{
    /// <summary>
    /// Utils for using JSON
    /// </summary>
    public class Json
    {
        /// <summary>
        /// Deserializes a JSON file
        /// </summary>
        /// <typeparam name="type">The type to deserialize to</typeparam>
        /// <param name="fileName">Filename of the file to deserialize</param>
        /// <param name="ignoreError">Whether to be silent on error</param>
        /// <param name="errorActions">errorActions to pass to showError</param>
        /// <returns>Deserialized object</returns>
        public static type DeserializeFile<type>(string fileName, bool ignoreError = false, string[] errorActions = null)
        {
            StreamReader SR = null;
            try
            {
                SR = new StreamReader(fileName);
                type Deserialized = JsonConvert.DeserializeObject<type>(SR.ReadToEnd());
                SR.Close();
                return Deserialized;
            }
            catch (Exception e)
            {
                try { SR.Close(); } catch { };
                if (!ignoreError) Logging.showError("Failed to parse JSON" + Environment.NewLine + e.ToString(), errorActions);
                return default(type);
            }
        }

        /// <summary>
        /// Deserializes string
        /// </summary>
        /// <typeparam name="type">Type to deserialize to</typeparam>
        /// <param name="serialiszedJson">the Json to deserialize</param>
        /// <param name="ignoreError">whether to be silent on error</param>
        /// <param name="errorActions">actions to pass to showError</param>
        /// <returns></returns>
        public static type DeserializeString<type>(string serialiszedJson, bool ignoreError = false, string[] errorActions = null)
        {
            try
            {
                return JsonConvert.DeserializeObject<type>(serialiszedJson);
            }
            catch (Exception e)
            {
                if (!ignoreError) Logging.showError("Failed to parse JSON" + Environment.NewLine + e.ToString(), errorActions);
                return default(type);
            }
        }

        /// <summary>
        /// Serializes a object into Formatted(indented) JSON
        /// </summary>
        /// <param name="value">Object to Serialize</param>
        /// <param name="ignoreError">Whether to be silent on error</param>
        /// <param name="errorActions"></param>
        /// <returns></returns>
        public static string SerializeFormatted(object value, bool ignoreError = false, string[] errorActions = null)
        {
            try
            {
                return JsonConvert.SerializeObject(value, Formatting.Indented);
            }
            catch (Exception e)
            {
                if (!ignoreError) Logging.showError("Failed to serialize JSON" + Environment.NewLine + e.ToString(), errorActions);
                return default(string);
            }
        }

        public static void SerializeToFile(object value, string fileName, bool ignoreError = false, string[] errorActions = null)
        {
            StreamWriter SW = null;
            try
            {
                SW = new StreamWriter(fileName);
                SW.Write(JsonConvert.SerializeObject(value, Formatting.Indented));
                SW.Close();
            }
            catch (Exception e)
            {
                try { SW.Close(); } catch { };
                if (!ignoreError) Logging.showError("Failed to serialize JSON" + Environment.NewLine + e.ToString(), errorActions);
            }
        }
    }
}
