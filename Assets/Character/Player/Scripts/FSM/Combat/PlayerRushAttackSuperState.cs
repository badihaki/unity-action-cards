using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRushAttackSuperState : PlayerCombatSuperState
{
    public PlayerRushAttackSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._AnimationController.SetBool("rush", true);
    }

    public override void ExitState()
    {
        base.ExitState();

        _PlayerCharacter._AnimationController.SetBool("rush", false);
    }
}
