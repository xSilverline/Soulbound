using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveSystem
    {
        private static string path = Application.persistentDataPath + "/save.savefile";

        public static void Save(SaveData data)
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            // Debug.Log("Save file created!"+ path);
        }

        public static SaveData Load()
        {
            if (!File.Exists(path)) return null;

            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }

        public static bool SaveExists()
        {
            return File.Exists(path);
        }
        
        public static void DeleteSave()
        {
            File.Delete(path);
        }
    }
}

