using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weaponless Attack A", menuName = "Characters/Create Attacks/Weaponless Attack A")]
public class PlayerWeaponlessAttackAState : PlayerAttackSuperState
{
    public PlayerWeaponlessAttackAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._AnimationController.SetBool("attackA", true);
        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _PlayerCharacter._AttackController.SetAttackParameters(1, 1.178f, 0.75f);
    }

    public override void ExitState()
    {
        base.ExitState();

        _PlayerCharacter._AnimationController.SetBool("attackA", false);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
     
        if (canCombo && attackInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._AttackB);
    }

    // end
}
