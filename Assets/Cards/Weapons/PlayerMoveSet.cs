using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Moveset", menuName = "Create Item/Weapons/New Player Moveset")]
public class PlayerMoveSet : MoveSetScriptableObject
{
    public PlayerAttackSuperState _AttackA;
    public PlayerAttackSuperState _FinisherA;
    public PlayerAttackSuperState _AttackB;
    public PlayerAttackSuperState _FinisherB;
    public PlayerAttackSuperState _AttackC;
    public PlayerAttackSuperState _FinisherC;
    public PlayerAttackSuperState _Special;
}
