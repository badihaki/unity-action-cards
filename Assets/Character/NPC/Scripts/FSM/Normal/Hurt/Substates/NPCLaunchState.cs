using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Hurt/Launch", fileName = "Launch")]
public class NPCLaunchState : NPCHurtSuperState
{
	private bool isBeingLaunched;

	public override void EnterState()
	{
		base.EnterState();
		isBeingLaunched = true;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		
		if (isBeingLaunched && Time.time > _StateEnterTime + 0.75f)
		{
			isBeingLaunched = false;
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		if (isBeingLaunched)
		{
			_NPC._MoveController.Launch();
			_NPC._MoveController.ApplyGravity(2f);
		}
		else
		{
			_NPC._MoveController.ApplyGravity();
		}
		_NPC._MoveController.ApplyExternalForces();
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (Time.time > _StateEnterTime + 1.358f)
		{
			if (_NPC._Actor._CheckGrounded.IsGrounded())
			{
				if(_NPC._NPCActor._AggressionManager.isAggressive)
					_StateMachine.ChangeState(_StateMachine._IdleAggressiveState);
				else
					_StateMachine.ChangeState(_StateMachine._IdleState);
			}
			else
				_StateMachine.ChangeState(_StateMachine._FallingState);
		}
	}
}
