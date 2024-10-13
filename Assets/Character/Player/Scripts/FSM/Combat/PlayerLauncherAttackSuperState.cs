using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLauncherAttackSuperState : PlayerCombatSuperState
{
    public PlayerLauncherAttackSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
}
