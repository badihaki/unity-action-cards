using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weaponless Attack C", menuName = "Characters/Create Attacks/Weaponless Attack C")]
public class PlayerWeaponlessAttackCState : PlayerAttackSuperState
{
    public PlayerWeaponlessAttackCState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
}
