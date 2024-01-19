using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

public static class SaveFileHandler
{
    public static void CopySaveDataToPersistentPath()
    {
        string streamingAssetsPath = Application.streamingAssetsPath;
        string persistentPath = Application.persistentDataPath;

        // Path to the original JSON file in streaming assets   
        string originalFilePath = Path.Combine(streamingAssetsPath, "TXTFiles/SavegameStandard.json");

        // Path to the copy of the JSON file in the persistent data path
        string copiedFilePath = Path.Combine(persistentPath, "SavegameCopy.json");

        // Copy the file to the persistent data path
        File.Copy(originalFilePath, copiedFilePath, true);
    }
    
    public static Dictionary<string, object> GetSaveFile(string dataType)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "SavegameCopy.json");
        Dictionary<string, object> saveData = new Dictionary<string, object>();

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return null;
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JArray saveArray = (JArray)jsonObject["SaveData"];

        if (saveArray == null)
        {
            Debug.LogError("SaveData array not found.");
            return null;
        }

        foreach (JObject saveObj in saveArray)
        {
            string objDataType = (string)saveObj["DataType"];

            if (objDataType == dataType)
            {
                var dict = new Dictionary<string, object>();

                foreach (var property in saveObj.Properties())
                {
                    dict[property.Name] = property.Value.ToObject<object>();
                }

                return dict;
            }
        }

        Debug.LogError($"DataType '{dataType}' not found.");
        return null;
    }
    
    public static void UpdateSaveData(string dataType, string key, object newValue)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "SavegameCopy.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JArray saveArray = (JArray)jsonObject["SaveData"];

        if (saveArray == null)
        {
            Debug.LogError("SaveData array not found.");
            return;
        }

        foreach (JObject saveObj in saveArray)
        {
            string objDataType = (string)saveObj["DataType"];

            if (objDataType == dataType)
            {
                if (saveObj[key] != null)
                {
                    saveObj[key] = JToken.FromObject(newValue);

                    // Write the updated JSON back to the file
                    File.WriteAllText(filePath, jsonObject.ToString());
                    return;
                }
                else
                {
                    Debug.LogError($"Key '{key}' not found for DataType '{dataType}'.");
                    return;
                }
            }
        }

        Debug.LogError($"DataType '{dataType}' not found.");
    }
    
    public static void IncrementSaveData(string dataType, string key, int incrementBy)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "SavegameCopy.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JArray saveArray = (JArray)jsonObject["SaveData"];

        if (saveArray == null)
        {
            Debug.LogError("SaveData array not found.");
            return;
        }

        foreach (JObject saveObj in saveArray)
        {
            string objDataType = (string)saveObj["DataType"];

            if (objDataType == dataType)
            {
                if (saveObj[key] != null)
                {
                    int currentValue = (int)saveObj[key];
                    saveObj[key] = currentValue + incrementBy;

                    // Write the updated JSON back to the file
                    File.WriteAllText(filePath, jsonObject.ToString());
                    return;
                }
                else
                {
                    Debug.LogError($"Key '{key}' not found for DataType '{dataType}'.");
                    return;
                }
            }
        }

        Debug.LogError($"DataType '{dataType}' not found.");
    }
}
