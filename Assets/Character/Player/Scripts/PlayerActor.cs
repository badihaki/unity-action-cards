using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    private PlayerCharacter Character;

    public void Initialize(PlayerCharacter character)
    {
        Character = character;
    }

    public void AnimationTrigger()=> Character.StateTrigger();
    public void StateAnimationFinished() => Character.StateAnimationFinished();
}
