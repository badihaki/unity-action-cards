using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Item/Weapons/New Weapon")]
public class UnarmedDefaultWeapon : ScriptableObject
{
    public GameObject _WeaponGameObjectL;
    public GameObject _WeaponGameObjectR;
    public PlayerMoveSet _PlayerMoves;
    public enum WeaponType
    {
        unarmed,
        singleHandNormal,
        twoHandedBiggies,
        spear
    }
    public WeaponType _WeaponType;
}
