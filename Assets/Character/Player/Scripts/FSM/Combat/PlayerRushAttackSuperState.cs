using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRushAttackSuperState : PlayerCombatSuperState
{
    public PlayerRushAttackSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
}
