using System.Linq;
using UnityEngine;

public class NPCFarKnockbackState : NPCState
{
	private float knockbackTime;
	private Transform knockingFrom;

	public NPCFarKnockbackState()
	{
	}

	public override void EnterState()
	{
		base.EnterState();
		knockbackTime = 0.0f;
		knockingFrom = _NPC._NPCActor._AggressionManager._LastAggressors.Last();
		_StateMachine.LogFromState(knockingFrom.name);
	}
	public override void LogicUpdate()
	{
		base.LogicUpdate();
		knockbackTime += Time.deltaTime;
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		if (knockbackTime < 0.85f)
		{
			_NPC._MoveController.FarKnocBack(knockingFrom.position);
			_NPC._MoveController.ApplyGravity(knockbackTime);
			_NPC._MoveController.ApplyExternalForces();
		}
	}
	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if(Time.time >= _StateEnterTime + 1.15f)
		{
			_StateMachine.ChangeState(_StateMachine._FallingState);
		}
	}
}
