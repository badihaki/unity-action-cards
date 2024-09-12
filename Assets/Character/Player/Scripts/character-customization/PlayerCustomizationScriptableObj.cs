using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName ="Characters/Player/New Customization Item")]
public class PlayerCustomizationScriptableObj : ScriptableObject
{
    public string PartName;
    public bool isMaleBodyType = true; // if true, male, if false, female
    public enum CustomizationPart
    {
        head,
        horns,
        hair,
        top,
        hands,
        bottom,
        feet
    }
    public CustomizationPart CustomizationSlot;
    public Mesh Mesh;
    public Material Material;
}
