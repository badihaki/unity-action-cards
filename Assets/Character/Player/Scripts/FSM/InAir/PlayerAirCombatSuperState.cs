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

        _PlayerCharacter._MoveController.ZeroOutVertVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _PlayerCharacter._MoveController.ApplyGravity(0.15f);
        _PlayerCharacter._MoveController.MoveWithVerticalVelocity();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_PlayerCharacter._CheckGrounded.IsGrounded())
        {
            _PlayerCharacter._MoveController.SetDoubleJump(true);
            _PlayerCharacter._MoveController.SetAirDash(true);
            _StateMachine.ChangeState(_StateMachine._IdleState);
        }
        if (_AnimationIsFinished) _StateMachine.ChangeState(_StateMachine._FallingState);
    }
}
