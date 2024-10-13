using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Attack B", menuName = "Create Attacks/01_Sword/Sword Attack B")]
public class SwordAttackBState : PlayerAttackSuperState
{
    public SwordAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
}
