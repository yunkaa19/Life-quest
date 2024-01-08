using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public static class DataPersistenceTasks
{
    public static void SaveString(string fileName, string data)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.WriteAllText(filePath, data);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data to {filePath}: {e.Message}");
        }
    }

    public static string LoadString(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data from {filePath}: {e.Message}");
        }

        return null;
    }

    public static void SaveDateTime(string fileName, DateTime dateTime)
    {
        string formattedDate = dateTime.ToString("MMddyyyy_HHmmss");
        string sanitizedFileName = fileName.Replace(" ", "_"); // Replace spaces with underscores

        string filePath = Path.Combine(Application.persistentDataPath, $"{sanitizedFileName}_{formattedDate}");

        try
        {
            File.WriteAllText(filePath, formattedDate);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data to {filePath}: {e.Message}");
        }
    }


    public static DateTime LoadDateTime(string fileName)
    {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, $"{fileName}_*");

        if (filePaths.Length > 0)
        {
            string fileNameWithDate = Path.GetFileNameWithoutExtension(filePaths[0]);
            string formattedDate = fileNameWithDate.Substring(fileName.Length + 1); // Remove the filename prefix
            return DateTime.ParseExact(formattedDate, "MMddyyyy_HHmmss", CultureInfo.InvariantCulture);
        }

        return DateTime.MinValue;
    }

    public static void SaveList<T>(string fileName, List<T> list)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

        try
        {
            string json = JsonUtility.ToJson(new SerializableList<T>(list));
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data to {filePath}: {e.Message}");
        }
    }

    public static List<T> LoadList<T>(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<SerializableList<T>>(json)?.List ?? new List<T>();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data from {filePath}: {e.Message}");
        }

        return new List<T>();
    }

    [Serializable]
    private class SerializableList<T>
    {
        public List<T> List;

        public SerializableList(List<T> list)
        {
            List = list;
        }
    }
}
