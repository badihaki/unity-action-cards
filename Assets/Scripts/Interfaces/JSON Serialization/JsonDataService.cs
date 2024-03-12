using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonDataService : IDataServiceable
{
    public T LoadData<T>(string RelativePathj, bool Encrypted)
    {
        string path = Application.persistentDataPath + "/" + RelativePathj;

        if (!File.Exists(path))
        {
            Debug.LogError("No save data exists");
            throw new FileNotFoundException($"{path} does not exist. No data to load!");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception err)
        {
            Debug.LogError($"Can't load data because of : {err.Message} {err.StackTrace}");
            throw err;
        }
    }

    public bool SaveData<T>(string RelativePathj, T Data, bool Encrypted)
    {
        string path = Application.persistentDataPath + "/" + RelativePathj;

        try
        {
            if (File.Exists(path))
            {
                // delete old data, write new data
                Debug.Log("Data exists");
                File.Delete(path);
            }
            else
            {
                // write new data
                Debug.Log("New data being saved");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(Data));
            return true;

        }
        catch(Exception e)
        {
            Debug.LogError($"Unable to save due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    // end
}
