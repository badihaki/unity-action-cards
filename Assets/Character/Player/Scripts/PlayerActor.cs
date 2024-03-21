using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    public void InitializePlayerActor(PlayerCharacter character)
    {
        PCActor = character;

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
    }

    private void BuildActorBody(CharacterSaveData saveData)
    {
        CharCustomizationDatabase parts = GameManagerMaster.GameMaster.CharacterCustomizationDatabase;
        
        SkinnedMeshRenderer head = transform.Find("Model.Head").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer hair = transform.Find("Model.Hair").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer horns = transform.Find("Model.Horns").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer top = transform.Find("Model.Top").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer hands = transform.Find("Model.Hands").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer bottom = transform.Find("Model.Bottom").GetComponent<SkinnedMeshRenderer>();

        head.sharedMesh = parts.headDatabase[saveData.HeadIndex].mesh;
        head.material = parts.headDatabase[saveData.HeadIndex].material;

        hair.sharedMesh = parts.hairDatabase[saveData.HairIndex].mesh;
        hair.material = parts.hairDatabase[saveData .HairIndex].material;

        horns.sharedMesh = parts.hornsDatabase[saveData.HornIndex].mesh;
        horns.material = parts.hornsDatabase[saveData.HornIndex ].material;

        top.sharedMesh = parts.torsoDatabase[saveData.TopIndex].meshMale;
        top.material = parts.torsoDatabase[saveData.TopIndex].materialMale;

        hands.sharedMesh = parts.handsDatabase[saveData.HandsIndex].meshMale;
        hands.material = parts.handsDatabase[saveData .HandsIndex ].materialMale;

        bottom.sharedMesh = parts.bottomsDatabase[saveData.BottomIndex].meshMale;
        bottom.material = parts.bottomsDatabase[saveData .BottomIndex ].materialMale;
        /*GameObject head = Instantiate(parts.headDatabase[outfitData.HeadIndex].model, transform);
        GameObject hair = Instantiate(parts.hairDatabase[outfitData.HairIndex].model, transform);
        GameObject horns = Instantiate(parts.hornsDatabase[outfitData.HornIndex].model, transform);
        GameObject top;
        GameObject hands;
        GameObject bottoms;
        if (bodyType == bodyTypes.male)
        {
            top = Instantiate(parts.torsoDatabase[outfitData.TopIndex].meshMale, transform);
            hands = Instantiate(parts.handsDatabase[outfitData.HairIndex].meshMale, transform);
            bottoms =Instantiate(parts.bottomsDatabase[outfitData.BottomIndex].meshMale, transform);
        }
        else
        {
            top = Instantiate(parts.torsoDatabase[outfitData.TopIndex].meshfemale, transform);
            bottoms = Instantiate(parts.handsDatabase[outfitData.HairIndex].meshfemale, transform);
            hands = Instantiate(parts.bottomsDatabase[outfitData.BottomIndex].meshfemale, transform);
        }
        head.GetComponent<SkinnedMeshRenderer>().rootBone = rootBone;
        hair.GetComponent<SkinnedMeshRenderer>().rootBone = rootBone;
        horns.GetComponent<SkinnedMeshRenderer>().rootBone = rootBone;
        top.GetComponent<SkinnedMeshRenderer>().rootBone = rootBone;
        hands.GetComponent<SkinnedMeshRenderer>().rootBone = rootBone;
        bottoms.GetComponent<SkinnedMeshRenderer>().rootBone = rootBone;*/
        print("done loading maybe");
    }

    public void AnimationTrigger()=> PCActor.StateTrigger();
    public void StateAnimationFinished() => PCActor.StateAnimationFinished();
}
