using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Defense", menuName ="Create Defense Action/Dodge")]
public class PlayerDodgeState : PlayerDefenseSuperState
{
    public PlayerDodgeState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _PlayerCharacter._AnimationController.SetBool("dodge", true);
        _PlayerCharacter._AnimationController.applyRootMotion = true;
        _PlayerCharacter._PlayerActor.SetSyncParentMotion(true);
        
        _PlayerCharacter._Controls.UseDefense();

        // _PlayerCharacter._LocomotionController.RotateCharacter(moveInput);

        Vector3 rollDirection = Camera.main.transform.forward * moveInput;
        // rollDirection.y = 0;
        // rollDirection.Normalize();
        // 
        // Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
        // _PlayerCharacter.transform.rotation = playerRotation;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // gotta sync player with player.animationController (the animator) position and rotation
        // _PlayerCharacter.transform.position += _PlayerCharacter._AnimationController.deltaPosition;
        // _PlayerCharacter.transform.position += _PlayerCharacter._AnimationController.deltaPosition;
    }

    public override void ExitState()
    {
        base.ExitState();
        _PlayerCharacter._AnimationController.SetBool("dodge", false);
        _PlayerCharacter._AnimationController.applyRootMotion = false;
        _PlayerCharacter._PlayerActor.SetSyncParentMotion(false);

        // _PlayerCharacter.transform.position = _PlayerCharacter._Actor.position;
        // _PlayerCharacter._Actor.localPosition = _PlayerCharacter.transform.position;
    }

    public override void AnimationFinished()
    {
        base.AnimationFinished();

        _PlayerCharacter._StateMachine.ChangeState(_PlayerCharacter._IdleState);
    }
}
