using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Movement/Chase", fileName = "Chase")]
public class NPCChaseState : NPCState
{
	private float distanceFromPlayer;
	protected NPCAttackController _AttackController;

	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);
		_AttackController = npc._AttackController as NPCAttackController;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (_NPC._NPCActor._AggressionManager.isAggressive && _AttackController._ActiveTarget != null)
		{
			distanceFromPlayer = _AttackController.GetDistanceFromTarget();
		}
		else
		{
			distanceFromPlayer = 1.75f;
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		if (_NPC._NPCActor._AggressionManager.isAggressive)
		{
			_NPC._MoveController.SetAgentDestination(_AttackController._ActiveTarget.position); // set desired distance from here in enter state
			_NPC._MoveController.MoveToTarget();
			_NPC._MoveController.RotateTowardsTarget(_AttackController._ActiveTarget);
		}
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (_AttackController._ActiveTarget != null) // make sure we have a target
		{
			if (distanceFromPlayer < _NPC._MoveSet.GetCurrentAttack().desiredDistance)
				_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleAggressiveState);
		}
		if (!_NPC._NPCActor._AggressionManager.isAggressive)
		{
			// not aggressive

			if(_NPC._NavigationController._CurrentNavNode != null)
			{
				// make sure the current node isn't null, and...
				// check distance between current node from nav controller and actor
				if (Vector3.Distance(_NPC._NPCActor.transform.position, _NPC._NavigationController._CurrentNavNode.transform.position) <= _NPC._NavigationController._MaxDistance)
				{
					_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleState);
				}
			}
			else
			{
				_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleState);
			}
		}
	}

	// end
}
