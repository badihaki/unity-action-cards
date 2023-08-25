using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Item/Weapons/New Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public GameObject _Prefab;
    public PlayerMoveSet _PlayerMoves;
}
