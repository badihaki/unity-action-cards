using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedCardState : PlayerCardSuperState
{
    public PlayerGroundedCardState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (!cardInput) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
        if (!_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_PlayerCharacter._FallingState);
    }
}
