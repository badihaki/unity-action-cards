using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Defense", menuName ="Characters/Player/Create Defense Action/Dodge")]
public class PlayerDodgeState : PlayerDefenseSuperState
{
    public PlayerDodgeState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _PlayerCharacter._AnimationController.SetBool("dodge", true);
        
        _PlayerCharacter._Controls.UseDefense();
    }

    public override void ExitState()
    {
        base.ExitState();
        _PlayerCharacter._AnimationController.SetBool("dodge", false);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _PlayerCharacter._MoveController.ApplyGravity();
        _PlayerCharacter._MoveController.MoveWithVerticalVelocity();
    }

    public override void AnimationFinished()
    {
        base.AnimationFinished();

        _PlayerCharacter._StateMachine.ChangeState(_StateMachine._IdleState);
    }
}
