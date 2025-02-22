using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Idle/StalkingWait", fileName = "Stalking Idle")]
public class NPCEnemyStalkingIdleState : NPCIdleState
{
	[field: SerializeField]
	private float lookForTargetsTimer;

	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);
		lookForTargetsTimer = GameManagerMaster.GameMaster.Dice.RollD4();
		minimumWait = 1.0f;
		minimumWaitMax = 2.0f;
		maximumWaitMin = 2.2f;
		maximumWait = 12.0f;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		ManageLookTimer();
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
}
