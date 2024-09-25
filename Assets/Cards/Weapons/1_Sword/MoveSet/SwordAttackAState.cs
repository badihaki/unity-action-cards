using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Attack A", menuName = "Create Attacks/01_Sword/Sword Attack A")]
public class SwordAttackAState : PlayerAttackSuperState
{
    public SwordAttackAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
}
