using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Attack C", menuName = "Create Attacks/00_Unarmed/Unarmed Attack C")]
public class PlayerUnarmedAttackCState : PlayerAttackSuperState
{
    public PlayerUnarmedAttackCState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _PlayerCharacter._AttackController.SetAttackParameters(2, 2, 0.75f);
        ShowOrHideWeapon(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        ShowOrHideWeapon(false);
    }
}
