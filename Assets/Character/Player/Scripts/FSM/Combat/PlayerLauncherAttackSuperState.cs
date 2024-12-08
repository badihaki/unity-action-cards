using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLauncherAttackSuperState : PlayerCombatSuperState
{
    public PlayerLauncherAttackSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void ExitState()
    {
        base.ExitState();
        _AttackController.ResetAttackParameters();
		_PlayerCharacter._WeaponController.UseWeaponDurability(_PlayerCharacter._WeaponController._CurrentWeapon._DurabilitySpecialCost);
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (canCombo && jumpInput) _StateMachine.ChangeState(_StateMachine._JumpState);
	}
}
