using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weaponless Attack A", menuName = "Characters/Create Attacks/Weaponless Attack A")]
public class PlayerWeaponlessAttackAState : PlayerAttackSuperState
{
    public PlayerWeaponlessAttackAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
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
