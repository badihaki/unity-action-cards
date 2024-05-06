using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Moveset", menuName = "Create Item/Weapons/New Player Moveset")]
public class PlayerMoveSetScriptableObj : MoveSetScriptableObject
{
    public PlayerAttackSuperState _AttackA;
    public PlayerAttackSuperState _AttackB;
    public PlayerAttackSuperState _AttackC;
    public PlayerAttackSuperState _SpecialA;
    public PlayerAttackSuperState _SpecialB;
    public PlayerAttackSuperState _SpecialC;
}
