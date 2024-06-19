using System.IO;
using UnityEngine;

namespace Scripts.Util
{
    public class JsonController
    {
        public static void SaveDataToJson<T>(T data, string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            Debug.LogWarning("new Json file path: " + filePath);
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, jsonData);
        }

        public static T LoadDataFromJson<T>(string fileName) where T : new()
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            T data;
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                data = JsonUtility.FromJson<T>(jsonData);
            }
            else
                data = default(T);

            return data;
        }
    }
}