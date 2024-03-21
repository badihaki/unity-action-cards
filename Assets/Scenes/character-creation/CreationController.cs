using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

public class CreationController : MonoBehaviour
{
    [field: SerializeField] private SkinnedMeshRenderer head;
    [field: SerializeField] private int headIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer hair;
    [field: SerializeField] private int hairIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer horns;
    [field: SerializeField] private int hornsIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer top;
    [field: SerializeField] private int topIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer bottom;
    [field: SerializeField] private int bottomIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer hands;
    [field: SerializeField] private int handsIndex = 0;

    private CharCustomizationDatabase customizationDatabase;
    // Start is called before the first frame update
    void Start()
    {
        customizationDatabase = GameManagerMaster.GameMaster.CharacterCustomizationDatabase;
        head.sharedMesh = customizationDatabase.headDatabase[headIndex].mesh;
        head.material= customizationDatabase.headDatabase[headIndex].material;

        hair.sharedMesh = customizationDatabase.hairDatabase[hairIndex].mesh;
        hair.material = customizationDatabase.hairDatabase[hairIndex].material;

        horns.sharedMesh = customizationDatabase.hornsDatabase[hornsIndex].mesh;
        horns.material = customizationDatabase.hornsDatabase[hornsIndex].material;

        top.sharedMesh = customizationDatabase.torsoDatabase[topIndex].meshMale;
        top.material = customizationDatabase.torsoDatabase[topIndex].materialMale;
        
        hands.sharedMesh = customizationDatabase.handsDatabase[handsIndex].meshMale;
        hands.sharedMaterial = customizationDatabase.handsDatabase[handsIndex].materialMale;

        bottom.sharedMesh = customizationDatabase.bottomsDatabase[bottomIndex].meshMale;
        bottom.material = customizationDatabase.bottomsDatabase[bottomIndex].materialMale;
    }

    public void SelectNextBodyPart(string bodyPart)
    {
        switch(bodyPart)
        {
            case "top":
                topIndex++;
                if(topIndex > (customizationDatabase.torsoDatabase.Count - 1))
                {
                    topIndex = 0;
                }
                top.sharedMesh = customizationDatabase.torsoDatabase[topIndex].meshMale;
                top.material = customizationDatabase.torsoDatabase[topIndex].materialMale;
                break;
            case "bottom":
                bottomIndex++;
                if (bottomIndex > (customizationDatabase.bottomsDatabase.Count - 1))
                {
                    bottomIndex = 0;
                }
                bottom.sharedMesh = customizationDatabase.bottomsDatabase[bottomIndex].meshMale;
                bottom.material = customizationDatabase.bottomsDatabase[bottomIndex].materialMale;
                break;
        }
    }
    public void SelectPreviousBodyPart(string bodyPart)
    {
        switch (bodyPart)
        {
            case "top":
                topIndex--;
                if (topIndex < 0)
                {
                    topIndex = (customizationDatabase.torsoDatabase.Count - 1);
                }
                top.sharedMesh = customizationDatabase.torsoDatabase[topIndex].meshMale;
                top.material = customizationDatabase.torsoDatabase[topIndex].materialMale;
                break;
            case "bottom":
                bottomIndex--;
                if (bottomIndex < 0)
                {
                    bottomIndex = (customizationDatabase.bottomsDatabase.Count - 1);
                }
                bottom.sharedMesh = customizationDatabase.bottomsDatabase[bottomIndex].meshMale;
                bottom.material = customizationDatabase.bottomsDatabase[bottomIndex].materialMale;
                break;
        }
    }
    public void SaveChar()
    {
        CharacterSaveData save = new CharacterSaveData("Player", headIndex, hornsIndex, hairIndex, topIndex, handsIndex, bottomIndex);
        GameManagerMaster.GameMaster.SaveLoadManager.SaveCharacterData(save);
    }
}
