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
        throw new NotImplementedException();
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
