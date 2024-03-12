using System.Collections;
using System.Collections.Generic;
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
}

public class CharacterSaveData
{
    public string Name;
    public int HeadIndex, HornIndex, HairIndex, TopIndex, HandsIndex, BottomIndex;
    public CharacterSaveData(string name, int headIndex, int hornsIndex, int hairIndex, int topIndex, int handsIndex, int bottomIndex)
    {
        this.Name = name;
        this.HeadIndex = headIndex;
        this.HornIndex = hornsIndex;
        this.HairIndex = hairIndex;
        this.TopIndex = topIndex;
        this.HandsIndex = handsIndex;
        this.BottomIndex = bottomIndex;
    }
}