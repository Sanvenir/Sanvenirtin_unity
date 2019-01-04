using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ObjectScripts.CharSubstance;
using UnityEngine;

namespace UtilScripts
{
    public class JsonData
    {
        public static T LoadDataFromFile<T>(string dir, string name)
        {
            dir = Path.Combine(Application.dataPath, dir);
            var path = Path.Combine(dir, string.Format("{0}.json", name));
            
            if (!File.Exists(path))
            {
                return default(T);
            }

            var sr = new StreamReader(path);

            var json = sr.ReadToEnd();
            sr.Close();
            return json.Length > 0 ? JsonUtility.FromJson<T>(json) : default(T);
        }

        public static void SaveDataToFile(string dir, string name, object properties)
        {
            dir = Path.Combine(Application.dataPath, dir);
            var path = Path.Combine(dir, string.Format("{0}.json", name));
            
            Directory.CreateDirectory(dir);
            var sw = new StreamWriter(path);
            var json = JsonUtility.ToJson(properties, true);
            sw.Write(json);
            sw.Close();
        }
    }
}