using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

public class CreationController : MonoBehaviour
{
    [field: SerializeField] private SkinnedMeshRenderer head;
    [field: SerializeField] private SkinnedMeshRenderer[] head_parts;
    [field: SerializeField] private int headIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer hair;
    [field: SerializeField] private SkinnedMeshRenderer[] hair_parts;
    [field: SerializeField] private int hairIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer horns;
    [field: SerializeField] private SkinnedMeshRenderer[] horns_parts;
    [field: SerializeField] private int hornsIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer top;
    [field: SerializeField] private SkinnedMeshRenderer[] top_parts;
    [field: SerializeField] private int topIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer bottom;
    [field: SerializeField] private SkinnedMeshRenderer[] bottom_parts;
    [field: SerializeField] private int bottomIndex = 0;
    [field: SerializeField] private SkinnedMeshRenderer hands;
    [field: SerializeField] private SkinnedMeshRenderer[] hands_parts;
    [field: SerializeField] private int handsIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        // head = _head_parts[0];
        // hair = _hair_parts[0];
        // horns = _horns_parts[0];
        // top = GameObject.Find("Character").transform.Find("Model").Find("Top").GetComponent<SkinnedMeshRenderer>();
        // hands = _hands_parts[0];
        head.sharedMesh = head_parts[headIndex].sharedMesh;
        head.sharedMaterial = head_parts[headIndex].sharedMaterial;

        hair.sharedMesh = hair_parts[hairIndex].sharedMesh;
        hair.sharedMaterials = hair_parts[hairIndex].sharedMaterials;

        horns.sharedMesh = horns_parts[hornsIndex].sharedMesh;
        horns.sharedMaterials = horns_parts[hornsIndex].sharedMaterials;

        top.sharedMesh = top_parts[topIndex].sharedMesh;
        top.sharedMaterial = top_parts[topIndex].sharedMaterial;
        
        hands.sharedMesh = hands_parts[handsIndex].sharedMesh;
        hands.sharedMaterial = hands_parts[handsIndex].sharedMaterial;

        bottom.sharedMesh = bottom_parts[bottomIndex].sharedMesh;
        bottom.sharedMaterial = bottom_parts[bottomIndex].sharedMaterial;
    }

    public void SelectNextBodyPart(string bodyPart)
    {
        switch(bodyPart)
        {
            case "top":
                // print("max index number for tops is " + (top_parts.Length - 1).ToString());
                // print("top index is now " + (topIndex++).ToString());
                topIndex++;
                if(topIndex > (top_parts.Length - 1))
                {
                    topIndex = 0;
                }
                top.sharedMesh = top_parts[topIndex].sharedMesh;
                top.sharedMaterial = top_parts[topIndex].sharedMaterial;
                break;
            case "bottom":
                // print("max index number for tops is " + (top_parts.Length - 1).ToString());
                // print("top index is now " + (topIndex++).ToString());
                bottomIndex++;
                if (bottomIndex > (bottom_parts.Length - 1))
                {
                    bottomIndex = 0;
                }
                bottom.sharedMesh = bottom_parts[bottomIndex].sharedMesh;
                bottom.sharedMaterial = bottom_parts[bottomIndex].sharedMaterial;
                break;
        }
    }
    public void SelectPreviousBodyPart(string bodyPart)
    {
        switch (bodyPart)
        {
            case "top":
                // print("max index number for tops is " + (top_parts.Length - 1).ToString());
                // print("top index is now " + (topIndex++).ToString());
                topIndex--;
                if (topIndex < 0)
                {
                    topIndex = (top_parts.Length - 1);
                }
                top.sharedMesh = top_parts[topIndex].sharedMesh;
                top.sharedMaterial = top_parts[topIndex].sharedMaterial;
                break;
            case "bottom":
                // print("max index number for tops is " + (top_parts.Length - 1).ToString());
                // print("top index is now " + (topIndex++).ToString());
                bottomIndex--;
                if (bottomIndex < 0)
                {
                    bottomIndex = (bottom_parts.Length - 1);
                }
                bottom.sharedMesh = bottom_parts[bottomIndex].sharedMesh;
                bottom.sharedMaterial = bottom_parts[bottomIndex].sharedMaterial;
                break;
        }
    }
    public void SaveChar()
    {
        // System.Object charSaveData = new System.Object();

        /*
        dynamic characterSaveData = new ExpandoObject();
        characterSaveData.headIndex = headIndex;
        characterSaveData.hornsIndex = hornsIndex;
        characterSaveData.hairIndex = hairIndex;
        characterSaveData.topIndex = topIndex;
        characterSaveData.handsIndex = handsIndex;
        characterSaveData.bottomIndex = bottomIndex;
        */
        CharacterSaveData save = new CharacterSaveData("Player", headIndex, hornsIndex, hairIndex, topIndex, handsIndex, bottomIndex);
        GameManagerMaster.GameMaster.SaveLoadManager.SaveCharacterData(save);
    }
}
