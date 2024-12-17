using System;
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
        if (GameManagerMaster.GameMaster.logExtraNPCData)
            print(">>>>>> Subscribed to entity is damaged event");
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

	public override void Damage(int damage, Transform damageSource, bool knockback = false, bool launched = false, Character damageSourceController = null)
	{
        EntityIsDamaged(CalculateAggression(damage, knockback, launched), damageSource);
		base.Damage(damage, damageSource, knockback, launched, damageSourceController);
	}

	private int CalculateAggression(int damage, bool knocked, bool launched)
	{
		float aggressionCalculation = damage * Mathf.Sqrt(100 - _AggressionManager._Aggression);
		//print($"first calculation = {aggressionCalculation}");

        //if (knockForce < launchForce)
        //    aggressionCalculation *= (float)Math.Sqrt(knockForce * launchForce);
        //else
        //    aggressionCalculation *= (float)Math.Sqrt(launchForce * knockForce);
		//print($"aggr calculation w/ knock * (health * launch) = {aggressionCalculation}");
        if(knocked || launched)
        {
            aggressionCalculation = 100.0f;
        }
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
