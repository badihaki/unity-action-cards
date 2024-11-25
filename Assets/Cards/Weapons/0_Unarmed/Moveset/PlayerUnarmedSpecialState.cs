using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Special", menuName = "Characters/Player/Create Attacks/00_Unarmed/Unarmed Special")]
public class PlayerUnarmedSpecialState : PlayerSpecialSuperState
{
    public PlayerUnarmedSpecialState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        ShowOrHideWeapon(true);
		_AttackController.SetAttackParameters(0.78f, 1.75f, 1);
	}

}
