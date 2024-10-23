using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActor : Actor, ITargetable
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

    public Transform GetTargetable() => transform;

	public override void ApplyKnockback(Transform forceSource, float knockforce, float launchForce)
	{
		base.ApplyKnockback(forceSource, knockforce, launchForce);

		NPC._MoveController.SetExternalForces(forceSource, knockforce, launchForce);
	}
}
