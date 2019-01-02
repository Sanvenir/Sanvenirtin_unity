using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ObjectScripts.CharSubstance;
using UnityEngine;

namespace UtilScripts
{
    public class JsonData
    {
        public static Properties LoadCharacterPropertiesFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var sr = new StreamReader(path);

            var json = sr.ReadToEnd();
            sr.Close();
            return json.Length > 0 ? JsonUtility.FromJson<Properties>(json) : null;
        }

        public static void SaveCharacterPropertiesToFile(string path, Properties properties)
        {
            var sw = new StreamWriter(path);
            var json = JsonUtility.ToJson(properties, true);
            sw.Write(json);
            sw.Close();
        }
    }
}