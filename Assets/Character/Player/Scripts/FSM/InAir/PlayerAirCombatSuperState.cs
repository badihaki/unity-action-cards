using UnityEngine;

public class PlayerAirCombatSuperState : PlayerAttackSuperState
{
    public PlayerAirCombatSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
    private float fallTime;

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._LocomotionController.ZeroOutVertVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _PlayerCharacter._LocomotionController.ApplyGravity(0.15f);
        _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_StateMachine._IdleState);
        if (_AnimationIsFinished) _StateMachine.ChangeState(_StateMachine._FallingState);
    }
}
