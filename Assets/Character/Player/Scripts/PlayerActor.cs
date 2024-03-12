using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    private PlayerCharacter PCActor;

    public void InitializePlayerActor(PlayerCharacter character)
    {
        PCActor = character;

        try
        {
            CharacterSaveData loadedOutfit = GameManagerMaster.GameMaster.SaveLoadManager.LoadCharacterData();
            print(loadedOutfit);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void AnimationTrigger()=> PCActor.StateTrigger();
    public void StateAnimationFinished() => PCActor.StateAnimationFinished();
}
