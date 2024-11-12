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
		_AggressionManager = GetComponent<NPCAggressionManager>();
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

	public override void Damage(int damage, Transform damageSource, float knockForce, float launchForce)
	{
		base.Damage(damage, damageSource, knockForce, launchForce);
        EntityIsDamaged(CalculateAggression(damage, knockForce, launchForce), damageSource);
	}

	private int CalculateAggression(int damage, float knockForce, float launchForce)
	{
        float aggressionCalculation = damage / NPC._CharacterSheet._StartingHealth;
		print($"first calculation = {aggressionCalculation}");

		aggressionCalculation *= (knockForce * (NPC._Health._CurrentHealth * launchForce));
		print($"aggr calculation w/ knock * (health * launch) = {aggressionCalculation}");

		int aggression = (int)Math.Ceiling(aggressionCalculation);
		print($"final aggression = {aggression}");
		return aggression;
	}
}
