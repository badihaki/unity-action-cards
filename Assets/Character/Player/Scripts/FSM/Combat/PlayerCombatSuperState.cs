using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSuperState : PlayerState
{
    public PlayerCombatSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public Vector2 moveInput { get; private set; }
    public Vector2 aimInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool attackInput { get; private set; }
    public bool readySpellInput { get; private set; }

    public override void CheckInputs()
    {
        base.CheckInputs();
        moveInput = _PlayerCharacter._Controls._MoveInput;
        aimInput = _PlayerCharacter._Controls._AimInput;
        jumpInput = _PlayerCharacter._Controls._JumpInput;
        attackInput = _PlayerCharacter._Controls._AttackInput;
        readySpellInput = _PlayerCharacter._Controls._ReadySpellInput;
    }
}
