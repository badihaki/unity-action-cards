using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirCardState : PlayerCardSuperState
{
    public PlayerInAirCardState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (!cardInput) _StateMachine.ChangeState(_StateMachine._FallingState);
        if (_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_StateMachine._IdleState);
    }
}
