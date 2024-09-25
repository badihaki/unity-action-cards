using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Item/Weapons/New Weapon")]
public class WeaponScriptableObj : ScriptableObject
{
    public GameObject _WeaponGameObjectL;
    public GameObject _WeaponGameObjectR;
    public PlayerMoveSetScriptableObj _PlayerMoves;
    public enum WeaponType
    {
        unarmed,
        sword,
        bigSword
    }
    public WeaponType _WeaponType;
    public GameObject _WeaponAttackFX;
    public GameObject _WeaponHitSpark;
}
