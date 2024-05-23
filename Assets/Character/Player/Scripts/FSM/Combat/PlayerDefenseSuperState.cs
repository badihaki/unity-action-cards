using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseSuperState : PlayerCombatSuperState
{
    public PlayerDefenseSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
}
