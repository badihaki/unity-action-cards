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
        if (GameManagerMaster.GameMaster.GMSettings.logExtraNPCData)
            print(">>>>>> Subscribed to entity is damaged event");
    }

	private void OnEnable()
	{
        if (_AggressionManager != null)
		EntityIsDamaged += _AggressionManager.AddAggression;
	}
	private void OnDisable()
	{
		EntityIsDamaged -= _AggressionManager.AddAggression;
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

	public override void TakeDamage(Damage dmgObj)
	{
        print(dmgObj.ToString());
        EntityIsDamaged(CalculateAggression(dmgObj.damageAmount, dmgObj.intendedResponse), dmgObj.damageSource);
        base.TakeDamage(dmgObj);
	}

	//private int CalculateAggression(int damage, bool knocked, bool launched)
	private int CalculateAggression(int damage, responsesToDamage dmgResponse)
	{
		float aggressionCalculation = damage * Mathf.Sqrt(100 - _AggressionManager._Aggression);
		//print($"first calculation = {aggressionCalculation}");

        //if (knockForce < launchForce)
        //    aggressionCalculation *= (float)Math.Sqrt(knockForce * launchForce);
        //else
        //    aggressionCalculation *= (float)Math.Sqrt(launchForce * knockForce);
		//print($"aggr calculation w/ knock * (health * launch) = {aggressionCalculation}");
        if(dmgResponse == responsesToDamage.knockBack || dmgResponse == responsesToDamage.launch)
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
