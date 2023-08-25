using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttack1State : PlayerAttackSuperState
{
    public PlayerNormalAttack1State(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(canCombo && attackInput)
        {
            // go to attack 2 state
        }
    }

    // end
}
