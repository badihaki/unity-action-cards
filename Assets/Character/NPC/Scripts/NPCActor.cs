using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActor : Actor
{
    private NonPlayerCharacter NPC;

    public override void Initialize(Character character)
    {
        base.Initialize(character);

        NPC = GetComponentInParent<NonPlayerCharacter>();
    }

    public override void StateAnimationFinished()
    {
        base.StateAnimationFinished();

        NPC.StateAnimEnd();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        NPC.StateSideEffect();
    }

    public override void AnimationVFXTrigger()
    {
        base.AnimationVFXTrigger();

        NPC.StateVisFX();
    }

    public override void AnimationSFXTrigger()
    {
        base.AnimationSFXTrigger();

        NPC.StateSideEffect();
    }
}
