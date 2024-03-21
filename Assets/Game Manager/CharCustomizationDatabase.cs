using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCustomizationDatabase : MonoBehaviour
{
    [Header("Outfit Options")]
    public List<ClothingSO> torsoDatabase;
    public List<ClothingSO> handsDatabase;
    public List<ClothingSO> bottomsDatabase;

    [Header("Body Options")]
    public List<BodyPartSO> headDatabase;
    public List<BodyPartSO> hairDatabase;
    public List<BodyPartSO> hornsDatabase;
}
