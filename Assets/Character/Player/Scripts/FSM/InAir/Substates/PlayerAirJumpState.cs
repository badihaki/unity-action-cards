using UnityEngine;

public class PlayerAirJumpState : PlayerJumpState
{
    public PlayerAirJumpState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    // bool isIncreasingVertVel;

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._LocomotionController.SetDoubleJump(false);
        // _PlayerCharacter._LocomotionController.ZeroOutVertVelocity();
        // isIncreasingVertVel = true;
        /*
        _PlayerCharacter._Controls.UseJump();
        _PlayerCharacter._LocomotionController.Jump();
        _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
         */
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        /*
        if(isIncreasingVertVel)
        {
            if(Time.time >= _StateEnterTime + 0.15f)
            {
                isIncreasingVertVel = false;
            }
        }
         */
    }
}
