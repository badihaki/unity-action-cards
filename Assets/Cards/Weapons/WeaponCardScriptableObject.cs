using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Card/Weapons/New Weapon Card")]
public class WeaponCardScriptableObject : CardScriptableObj
{
    [Header("Weapon Game Object")]
    public WeaponScriptableObject _Weapon;
    [Header("Weapon Durability Stats")]
    public float _WeaponDurability;
    public float _WeaponAttackDurabilityCost;
    public bool _NoDurabilityTimer;
}
