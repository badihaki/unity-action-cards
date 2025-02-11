using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Rush", menuName = "Characters/Player/Create Attacks/00_Unarmed/Unarmed Rush Attack")]
public class PlayerUnarmedRushState : PlayerRushAttackSuperState
{
    public PlayerUnarmedRushState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //_AttackController.SetAttackParameters(true, false, 1);
		_AttackController.SetAttackParameters(responsesToDamage.stagger);
	}
}
