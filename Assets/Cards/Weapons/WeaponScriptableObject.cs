using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Item/Weapons/New Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public GameObject _Prefab;
    public PlayerMoveSet _PlayerMoves;
    public enum WeaponType
    {
        unarmed,
        sword,
        spear
    }
    public WeaponType _WeaponType;
    public GameObject _WeaponGameObjectL;
    public GameObject _WeaponGameObjectR;
}
