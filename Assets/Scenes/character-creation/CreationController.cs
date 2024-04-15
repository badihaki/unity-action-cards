using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreationController : MonoBehaviour
{
    [Header("Male")]
    private GameObject mActor;
    private SkinnedMeshRenderer mHead;
    [field: SerializeField] private int mHeadIndex = 0;
    private SkinnedMeshRenderer mHair;
    [field: SerializeField] private int mHairIndex = 0;
    private SkinnedMeshRenderer mHorns;
    [field: SerializeField] private int mHornsIndex = 0;
    private SkinnedMeshRenderer mTop;
    [field: SerializeField] private int mTopIndex = 0;
    private SkinnedMeshRenderer mBottom;
    [field: SerializeField] private int mBottomIndex = 0;
    private SkinnedMeshRenderer mHands;
    [field: SerializeField] private int mHandsIndex = 0;

    [Header("FeMale")]
    private GameObject fActor;
    private SkinnedMeshRenderer fHead;
    [field: SerializeField] private int fHeadIndex = 0;
    private SkinnedMeshRenderer fHair;
    [field: SerializeField] private int fHairIndex = 0;
    private SkinnedMeshRenderer fHorns;
    [field: SerializeField] private int fHornsIndex = 0;
    private SkinnedMeshRenderer fTop;
    [field: SerializeField] private int fTopIndex = 0;
    private SkinnedMeshRenderer fBottom;
    [field: SerializeField] private int fBottomIndex = 0;
    private SkinnedMeshRenderer fHands;
    [field: SerializeField] private int fHandsIndex = 0;

    [field: SerializeField] private bool isMaleBody;

    private CharCustomizationDatabase customizationDatabase;
    // Start is called before the first frame update
    void Start()
    {
        customizationDatabase = GameManagerMaster.GameMaster.CharacterCustomizationDatabase;

        // male
        mActor = GameObject.Find("Male").gameObject;

        mHead = mActor.transform.Find("Head").GetComponent<SkinnedMeshRenderer>();
        mHead.sharedMesh = customizationDatabase.mHeadDatabase[mHeadIndex].mesh;
        mHead.material= customizationDatabase.mHeadDatabase[mHeadIndex].material;

        mHair = mActor.transform.Find("Hair").GetComponent<SkinnedMeshRenderer>();
        mHair.sharedMesh = customizationDatabase.mHairDatabase[mHairIndex].mesh;
        mHair.material = customizationDatabase.mHairDatabase[mHairIndex].material;

        mHorns = mActor.transform.Find("Horns").GetComponent<SkinnedMeshRenderer>();
        mHorns.sharedMesh = customizationDatabase.mHornsDatabase[mHornsIndex].mesh;
        mHorns.material = customizationDatabase.mHornsDatabase[mHornsIndex].material;

        mTop = mActor.transform.Find("Top").GetComponent<SkinnedMeshRenderer>();
        mTop.sharedMesh = customizationDatabase.mTorsoDatabase[mTopIndex].mesh;
        mTop.material = customizationDatabase.mTorsoDatabase[mTopIndex].material;

        mHands = mActor.transform.Find("Hands").GetComponent<SkinnedMeshRenderer>();
        mHands.sharedMesh = customizationDatabase.mHandsDatabase[mHandsIndex].mesh;
        mHands.sharedMaterial = customizationDatabase.mHandsDatabase[mHandsIndex].material;

        mBottom = mActor.transform.Find("Bottom").GetComponent<SkinnedMeshRenderer>();
        mBottom.sharedMesh = customizationDatabase.mBottomsDatabase[mBottomIndex].mesh;
        mBottom.material = customizationDatabase.mBottomsDatabase[mBottomIndex].material;
        mActor.GetComponent<Animator>().SetBool("cinematics", true);

        // female
        fActor = GameObject.Find("Female").gameObject;

        fHead = fActor.transform.Find("Head").GetComponent<SkinnedMeshRenderer>();
        fHead.sharedMesh = customizationDatabase.fHeadDatabase[fHeadIndex].mesh;
        fHead.material = customizationDatabase.fHeadDatabase[fHeadIndex].material;

        fHair = fActor.transform.Find("Hair").GetComponent<SkinnedMeshRenderer>();
        fHair.sharedMesh = customizationDatabase.fHairDatabase[fHairIndex].mesh;
        fHair.material = customizationDatabase.fHairDatabase[fHairIndex].material;

        fHorns = fActor.transform.Find("Horns").GetComponent<SkinnedMeshRenderer>();
        fHorns.sharedMesh = customizationDatabase.fHornsDatabase[fHornsIndex].mesh;
        fHorns.material = customizationDatabase.fHornsDatabase[fHornsIndex].material;

        fTop = fActor.transform.Find("Top").GetComponent<SkinnedMeshRenderer>();
        fTop.sharedMesh = customizationDatabase.fTorsoDatabase[fTopIndex].mesh;
        fTop.material = customizationDatabase.fTorsoDatabase[fTopIndex].material;

        fHands = fActor.transform.Find("Hands").GetComponent<SkinnedMeshRenderer>();
        fHands.sharedMesh = customizationDatabase.fHandsDatabase[fHandsIndex].mesh;
        fHands.sharedMaterial = customizationDatabase.fHandsDatabase[fHandsIndex].material;

        fBottom = fActor.transform.Find("Bottom").GetComponent<SkinnedMeshRenderer>();
        fBottom.sharedMesh = customizationDatabase.fBottomsDatabase[fBottomIndex].mesh;
        fBottom.material = customizationDatabase.fBottomsDatabase[fBottomIndex].material;
        fActor.GetComponent<Animator>().SetBool("cinematics", true);


        fActor.SetActive(false);
        isMaleBody = true;
    }

    private void ChangeBodyPart(string bodyPart, bool isSelectingNext)
    {
        if (isMaleBody)
        {
            switch (bodyPart)
            {
                case "top":
                    if (customizationDatabase.mTorsoDatabase.Count <= 1) break;
                    mTopIndex = isSelectingNext ? (mTopIndex + 1) : (mTopIndex - 1);
                    if (mTopIndex > (customizationDatabase.mTorsoDatabase.Count - 1))
                    {
                        mTopIndex = 0;
                    }
                    else if (mTopIndex < 0)
                    {
                        mTopIndex = (customizationDatabase.mTorsoDatabase.Count - 1);
                    }
                    mTop.sharedMesh = customizationDatabase.mTorsoDatabase[mTopIndex].mesh;
                    mTop.material = customizationDatabase.mTorsoDatabase[mTopIndex].material;
                    break;
                case "bottom":
                    if (customizationDatabase.mBottomsDatabase.Count <= 1) break;
                    mBottomIndex = isSelectingNext ? (mBottomIndex + 1) : (mBottomIndex - 1);
                    if (mBottomIndex > (customizationDatabase.mBottomsDatabase.Count - 1))
                    {
                        mBottomIndex = 0;
                    }
                    else if (mBottomIndex < 0)
                    {
                        mBottomIndex = (customizationDatabase.mBottomsDatabase.Count - 1);
                    }
                    mBottom.sharedMesh = customizationDatabase.mBottomsDatabase[mBottomIndex].mesh;
                    mBottom.material = customizationDatabase.mBottomsDatabase[mBottomIndex].material;
                    break;
            }
        }
        else
        {
            switch (bodyPart)
            {
                case "top":
                    if (customizationDatabase.fTorsoDatabase.Count <= 1) break;
                    fTopIndex = isSelectingNext ? (fTopIndex + 1) : (fTopIndex - 1);
                    if (fTopIndex > (customizationDatabase.mTorsoDatabase.Count - 1))
                    {
                        fTopIndex = 0;
                    }
                    else if (fTopIndex < 0)
                    {
                        fTopIndex = (customizationDatabase.mTorsoDatabase.Count - 1);
                    }
                    fTop.sharedMesh = customizationDatabase.mTorsoDatabase[fTopIndex].mesh;
                    fTop.material = customizationDatabase.mTorsoDatabase[fTopIndex].material;
                    break;
                case "bottom":
                    if (customizationDatabase.fBottomsDatabase.Count <= 1) break;
                    fBottomIndex = isSelectingNext ? (fBottomIndex + 1) : (fBottomIndex - 1);
                    if (fBottomIndex > (customizationDatabase.mBottomsDatabase.Count - 1))
                    {
                        fBottomIndex = 0;
                    }
                    else if (fBottomIndex < 0)
                    {
                        fBottomIndex = (customizationDatabase.mBottomsDatabase.Count - 1);
                    }
                    fBottom.sharedMesh = customizationDatabase.mBottomsDatabase[fBottomIndex].mesh;
                    fBottom.material = customizationDatabase.mBottomsDatabase[fBottomIndex].material;
                    break;
            }
        }
    }

    public void ChangeCharacterGender()
    {
        isMaleBody = !isMaleBody;

        mActor.SetActive(isMaleBody);
        fActor.SetActive(!isMaleBody);

        SetCharacterCinematicAnimation(isMaleBody);
    }

    private void SetCharacterCinematicAnimation(bool animateMale)
    {
        if(animateMale) mActor.GetComponent<Animator>().SetBool("cinematics", true);
        else fActor.GetComponent<Animator>().SetBool("cinematics", true); ;
    }

    public void SelectNextBodyPart(string bodyPart)
    {
        ChangeBodyPart(bodyPart, true);
    }
    public void SelectPreviousBodyPart(string bodyPart)
    {
        print(bodyPart);
        print("going down");
        ChangeBodyPart(bodyPart, false);
    }
    public void SaveChar()
    {
        CharacterSaveData save;
        if (isMaleBody)
            save = new CharacterSaveData(isMaleBody, "Player", mHeadIndex, mHornsIndex, mHairIndex, mTopIndex, mHandsIndex, mBottomIndex);
        else
            save = new CharacterSaveData(isMaleBody, "Player", fHeadIndex, fHornsIndex, fHairIndex, fTopIndex, fHandsIndex, fBottomIndex);
        GameManagerMaster.GameMaster.SaveLoadManager.SaveCharacterData(save);
    }
}
