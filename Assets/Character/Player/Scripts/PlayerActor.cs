using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerActor : Actor
{
    private PlayerCharacter PCActor;
    public enum bodyTypes
    {
        male,
        female
    }
    public bodyTypes bodyType { get; private set; }
    [field: SerializeField] private Transform rootBone;

    [field: SerializeField, Header("Animator Movement")]
    public Vector3 animatorMovementVector { get; private set; }
    [field: SerializeField] public Animator animationController { get; private set; }
    [field: SerializeField] private bool controlByRootMotion = false;

    public void InitializePlayerActor(PlayerCharacter character)
    {
        PCActor = character;
        animationController = GetComponent<Animator>();
/*
        if (PCActor._LoadNewOnStart)
        {
            try
            {
                CharacterSaveData loadedOutfit = GameManagerMaster.GameMaster.SaveLoadManager.LoadCharacterData();
                BuildActorBody(loadedOutfit);
            }
            catch (Exception err)
            {
                Debug.LogError($"Exception was thrown: {err}");
            }
        }
*/
    }

    private void BuildActorBody(CharacterSaveData saveData)
    {
        CharCustomizationDatabase parts = GameManagerMaster.GameMaster.CharacterCustomizationDatabase;

        if (saveData.isMale)
        {
            SkinnedMeshRenderer head = transform.Find("Model.Head").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer hair = transform.Find("Model.Hair").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer horns = transform.Find("Model.Horns").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer top = transform.Find("Model.Top").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer hands = transform.Find("Model.Hands").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer bottom = transform.Find("Model.Bottom").GetComponent<SkinnedMeshRenderer>();

            head.sharedMesh = parts.mHeadDatabase[saveData.HeadIndex].mesh;
            head.material = parts.mHeadDatabase[saveData.HeadIndex].material;

            hair.sharedMesh = parts.mHairDatabase[saveData.HairIndex].mesh;
            hair.material = parts.mHairDatabase[saveData .HairIndex].material;

            horns.sharedMesh = parts.mHornsDatabase[saveData.HornIndex].mesh;
            horns.material = parts.mHornsDatabase[saveData.HornIndex ].material;

            top.sharedMesh = parts.mTorsoDatabase[saveData.TopIndex].mesh;
            top.material = parts.mTorsoDatabase[saveData.TopIndex].material;

            hands.sharedMesh = parts.mHandsDatabase[saveData.HandsIndex].mesh;
            hands.material = parts.mHandsDatabase[saveData .HandsIndex ].material;

            bottom.sharedMesh = parts.mBottomsDatabase[saveData.BottomIndex].mesh;
            bottom.material = parts.mBottomsDatabase[saveData .BottomIndex ].material;
        }
            print("done loading maybe");
    }

    private void OnAnimatorMove()
    {
        print("On animator move");
        if (animationController && controlByRootMotion)
        {
            print("lets goooooo!!!!!!!!!!");
            animatorMovementVector = animationController.deltaPosition.normalized;
            PCActor.transform.position += animationController.deltaPosition;
            PCActor.transform.rotation = animationController.deltaRotation;
        }
    }

    public void SetSyncParentMotion(bool value) => controlByRootMotion = value;

    public void SendPositionDataToParent(PlayerCharacter parent)
    {
        OnAnimatorMove();
        // parent.transform.position += animationController.deltaPosition.normalized;
        parent.transform.position += animationController.deltaPosition;
    }

    public void AnimationTrigger()=> PCActor.StateTrigger();
    public void StateAnimationFinished() => PCActor.StateAnimationFinished();
}
