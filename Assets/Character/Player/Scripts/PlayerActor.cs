using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    private PlayerCharacter PCActor;

    public void InitializePlayerActor(PlayerCharacter character)
    {
        PCActor = character;
    }

    public void AnimationTrigger()=> PCActor.StateTrigger();
    public void StateAnimationFinished() => PCActor.StateAnimationFinished();
}
