using UnityEngine;

public class PlayerAirDashState : PlayerState
{
    public PlayerAirDashState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._LocomotionController.RotateInstantly(_PlayerCharacter._Controls._MoveInput);
        _PlayerCharacter._LocomotionController.SetAirDash(false);
		_PlayerCharacter._LocomotionController.Jump();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_AnimationIsFinished)
        {
            _StateMachine.ChangeState(_StateMachine._FallingState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

		_PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
    }

    public override void AnimationFinished()
    {
        base.AnimationFinished();

        _PlayerCharacter._StateMachine.ChangeState(_StateMachine._FallingState);
    }
}
