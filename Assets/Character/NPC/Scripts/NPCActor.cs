using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActor : Actor, ITargetable, IAggressable
{
    private NonPlayerCharacter NPC;

	[field: SerializeField, Header("Components")]
    public NPCAggressionManager _AggressionManager { get; protected set; }

	public event IAggressable.IsDamaged EntityIsDamaged;

	public override void Initialize(Character character)
    {
        base.Initialize(character);

        NPC = GetComponentInParent<NonPlayerCharacter>();

		// Aggression Management
		_AggressionManager = NPC.GetComponent<NPCAggressionManager>();
		_AggressionManager.Initialize(NPC);

		EntityIsDamaged += _AggressionManager.AddAggression;
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

	public override void Damage(int damage, Transform damageSource, bool knockback = false, bool launched = false, Character damageSourceController = null)
	{
		base.Damage(damage, damageSource, knockback, launched, damageSourceController);
        EntityIsDamaged(CalculateAggression(damage, knockback, launched), damageSource);
	}

	private int CalculateAggression(int damage, bool knockForce, bool launchForce)
	{
		float aggressionCalculation = damage * Mathf.Sqrt(100 - _AggressionManager._Aggression);
		//print($"first calculation = {aggressionCalculation}");

        //if (knockForce < launchForce)
        //    aggressionCalculation *= (float)Math.Sqrt(knockForce * launchForce);
        //else
        //    aggressionCalculation *= (float)Math.Sqrt(launchForce * knockForce);
		//print($"aggr calculation w/ knock * (health * launch) = {aggressionCalculation}");

		int aggression = (int)Math.Ceiling(aggressionCalculation);
		//print($"final aggression = {aggression}");
		return aggression;
	}

	public override void Die()
	{
		EntityIsDamaged -= _AggressionManager.AddAggression;

		base.Die();
	}
}
