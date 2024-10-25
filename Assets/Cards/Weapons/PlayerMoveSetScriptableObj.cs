using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Moveset", menuName = "Create Item/Weapons/New Player Moveset")]
public class PlayerMoveSetScriptableObj : MoveSetScriptableObject
{
    [Header("Light Attack Strings")]
    public PlayerAttackSuperState _AttackA;
    public PlayerAttackSuperState _AttackB;
    public PlayerAttackSuperState _AttackC;
    
    [Header("Special and Special Finishers")]
    public PlayerSpecialSuperState _Special;
    public PlayerSpecialSuperState _SpecialFinisherA;
    public PlayerSpecialSuperState _SpecialFinisherB;
    public PlayerSpecialSuperState _SpecialFinisherC;

    [Header("Air Attacks")]
    public PlayerAttackSuperState _AirAttackA;
    public PlayerAttackSuperState _AirAttackB;
    public PlayerAttackSuperState _AirAttackC;
    public PlayerAttackSuperState _AirSpecial;


    [Header("Universal Attacks")]
    public PlayerRushAttackSuperState _RushAttack;
    public PlayerLauncherAttackSuperState _LauncherAttack;

    [Header("Defensive Action")]
    public PlayerDefenseSuperState _Defense;
}
