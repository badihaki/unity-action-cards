using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCustomizationDatabase : MonoBehaviour
{
    [field: Header("Actor Bases")]
    [field: SerializeField] public GameObject mActorBase { get; private set; }
    [field: SerializeField] public GameObject fActorBase { get; private set; }

    [field: Header("Male")]
    [field: Header("Outfit Options")]
    [field: SerializeField] public List<ClothingSO> mTorsoDatabase { get; private set; }
    [field: SerializeField] public List<ClothingSO> mHandsDatabase { get; private set; }
    [field: SerializeField] public List<ClothingSO> mBottomsDatabase { get; private set; }

    [field: Header("Body Options")]
    [field: SerializeField] public List<BodyPartSO> mHeadDatabase { get; private set; }
    [field: SerializeField] public List<BodyPartSO> mHairDatabase { get; private set; }
    [field: SerializeField] public List<BodyPartSO> mHornsDatabase { get; private set; }

    [field: Header("Female")]
    [field: Header("Outfit Options")]
    [field: SerializeField] public List<ClothingSO> fTorsoDatabase { get; private set; }
    [field: SerializeField] public List<ClothingSO> fHandsDatabase { get; private set; }
    [field: SerializeField] public List<ClothingSO> fBottomsDatabase { get; private set; }

    [field: Header("Body Options")]
    [field: SerializeField] public List<BodyPartSO> fHeadDatabase { get; private set; }
    [field: SerializeField] public List<BodyPartSO> fHairDatabase { get; private set; }
    [field: SerializeField] public List<BodyPartSO> fHornsDatabase { get; private set; }
}
