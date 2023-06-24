using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadySpellState : PlayerCombatSuperState
{
    public PlayerReadySpellState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._CameraController.ResetCinemachineTargetTransform();
        _PlayerCharacter._CameraController.SwitchCam(_PlayerCharacter._CameraController._PlayerAimCamController);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);

        if (attackInput)
        {
            _PlayerCharacter._PlayerSpells.UseSpell();
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        _PlayerCharacter._CameraController.SwitchCam(_PlayerCharacter._CameraController._PlayerCamController);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (!readySpellInput)
        {
            if (_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
            else _StateMachine.ChangeState(_PlayerCharacter._FallingState);
        }
    }

    // end
}
