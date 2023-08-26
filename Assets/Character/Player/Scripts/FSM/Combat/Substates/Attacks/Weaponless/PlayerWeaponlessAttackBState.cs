using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weaponless Attack B", menuName = "Characters/Create Attacks/Weaponless Attack B")]
public class PlayerWeaponlessAttackBState : PlayerAttackSuperState
{
    public PlayerWeaponlessAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._AnimationController.SetBool("attackB", true);
        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
    }

    public override void ExitState()
    {
        base.ExitState();

        _PlayerCharacter._AnimationController.SetBool("attackB", false);
    }

    // end
}
