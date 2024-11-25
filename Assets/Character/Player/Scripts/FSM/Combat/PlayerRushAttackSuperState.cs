using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRushAttackSuperState : PlayerCombatSuperState
{
    public PlayerRushAttackSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void ExitState()
    {
        base.ExitState();
        _AttackController.ResetAttackParameters();
		_PlayerCharacter._WeaponController.UseWeaponDurability(_PlayerCharacter._WeaponController._CurrentWeapon._DurabilitySpecialCost);
	}
}
