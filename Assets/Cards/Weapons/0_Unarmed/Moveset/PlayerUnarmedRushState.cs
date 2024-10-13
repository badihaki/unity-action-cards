using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Rush", menuName = "Create Attacks/00_Unarmed/Unarmed Rush Attack")]
public class PlayerUnarmedRushState : PlayerRushAttackSuperState
{
    public PlayerUnarmedRushState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
}
