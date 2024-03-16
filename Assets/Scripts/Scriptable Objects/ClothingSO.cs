using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Outfit", menuName ="Create Item/Clothing Item")]
public class ClothingSO : ScriptableObject
{
    [Header("Clothing Stats")]
    public float weight = 0.0f;

    public enum outfitSlots
    {
        torso = 0,
        bottoms = 1,
        hands = 2
    }
    [Header("Gear Slot")]
    public outfitSlots outfitSlot;

    [Header("Bodytype: M")]
    public GameObject meshMale;
    public Material materialMale;
    [Header("Bodytype: F")]
    public GameObject meshfemale;
    public Material materialfemale;
    // end
}
