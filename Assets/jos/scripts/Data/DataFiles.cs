using System.IO;
using UnityEngine;

public static class DataFiles
{
    public static string GetPath(string fileName)
    {
        // Archivo en carpeta persistente del juego
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    public static void SaveJson<T>(string fileName, T data)
    {
        string path = GetPath(fileName);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        // Debug.Log($"[DataFiles] Guardado: {path}");
    }

    public static bool TryLoadJson<T>(string fileName, out T data) where T : class
    {
        string path = GetPath(fileName);
        if (!File.Exists(path))
        {
            data = null;
            return false;
        }

        string json = File.ReadAllText(path);
        if (string.IsNullOrWhiteSpace(json))
        {
            data = null;
            return false;
        }

        data = JsonUtility.FromJson<T>(json);
        return data != null;
    }
}
