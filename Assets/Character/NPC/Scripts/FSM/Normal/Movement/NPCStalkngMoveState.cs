using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Movement/Stalk", fileName = "Stalking Move")]
public class NPCStalkngMoveState : NPCMoveState
{
	[field: SerializeField]
	private float lookForTargetsTimer;
	private NPCAttackController attackController;

	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);
		lookForTargetsTimer = GameManagerMaster.GameMaster.Dice.RollD4();
		attackController = _NPC._AttackController as NPCAttackController;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		ManageLookTimer();
	}
	public override void CheckStateTransitions()
	{
		if(attackController._ActiveTarget != null)
		{
			_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleAggressiveState);
		}

		base.CheckStateTransitions();
	}

	private void ManageLookTimer()
	{
		if (lookForTargetsTimer > 0)
		{
			lookForTargetsTimer -= Time.deltaTime;
			if (lookForTargetsTimer < 0)
			{
				if (GameManagerMaster.GameMaster.GMSettings.logNPCUtilData)
					_StateMachine.LogFromState("Looking for an enemy");
				_NPC._EyeSight.LookForTargets();
				lookForTargetsTimer = GameManagerMaster.GameMaster.Dice.RollRandomDice(0.2f, 2.0f);
			}
		}
	}

	// end
}
