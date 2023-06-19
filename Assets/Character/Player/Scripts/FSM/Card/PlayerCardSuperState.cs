using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardSuperState : PlayerState
{
    public PlayerCardSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._CameraController.UnlockCursorKBM();
        _PlayerCharacter._PlayerCards.ShowHand();
    }

    public override void ExitState()
    {
        base.ExitState();

        _PlayerCharacter._PlayerCards.PutAwayHand();
        _PlayerCharacter._CameraController.LockCursorKBM();
    }

    public bool cardInput { get; private set; }


    public override void CheckInputs()
    {
        base.CheckInputs();

        cardInput = _PlayerCharacter._Controls._CardsInput;
    }
}
