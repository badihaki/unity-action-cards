using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager
{
    private IDataServiceable dataService = new JsonDataService();
    [field: SerializeField] private bool Encrypted;

    public void SaveCharacterData(CharacterSaveData data)
    {
        if (dataService.SaveData("/charsave.json", data, Encrypted))
        {
            Debug.Log("saved");
        }
        else
        {
            Debug.LogError("Did not save");
        }
    }

    public CharacterSaveData LoadCharacterData()
    {
        try
        {
            CharacterSaveData data = dataService.LoadData<CharacterSaveData>("/charsave.json", Encrypted);
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Can't load data due to {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    // end of the line
}

public class CharacterSaveData
{
    public string Name;
    public bool isMale;
    public int HeadIndex, HornIndex, HairIndex, TopIndex, HandsIndex, BottomIndex;
    public CharacterSaveData(bool isMale, string name, int headIndex, int hornsIndex, int hairIndex, int topIndex, int handsIndex, int bottomIndex)
    {
        this.isMale = isMale;
        this.Name = name;
        this.HeadIndex = headIndex;
        this.HornIndex = hornsIndex;
        this.HairIndex = hairIndex;
        this.TopIndex = topIndex;
        this.HandsIndex = handsIndex;
        this.BottomIndex = bottomIndex;
    }
}