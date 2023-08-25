using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Card/Weapons/New Weapon")]
public class WeaponScriptableObject : CardScriptableObj
{
    public float _WeaponDurability;
    public float _WeaponAttackDurabilityCost;
    public bool _NoDurabilityTimer;
    public MoveSetScriptableObjects _MoveSet;
}
