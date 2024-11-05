using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Special Finisher", menuName = "Create Attacks/00_Unarmed/Unarmed Special Finisher")]
public class PlayerUnarmedSpecialFinisherState : PlayerSpecialSuperState
{
    public PlayerUnarmedSpecialFinisherState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
		// _PlayerCharacter._AnimationController.SetBool("finisherA", true);

		_PlayerCharacter._AttackController.SetAttackParameters(0.15f, 1.75f, 2);
	}

    public override void ExitState()
    {
        base.ExitState();
        // _PlayerCharacter._AnimationController.SetBool("finisherA", false);
    }
}
