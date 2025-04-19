using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Item/Weapons/New Weapon")]
public class WeaponScriptableObj : ScriptableObject
{
    [Header("Game Objects")]
    public GameObject _WeaponGameObjectL;
    public GameObject _WeaponGameObjectR;

    [Header("Stats")]
    public int _Dmg = 1;
    public float _Force = 0.035f;
    
    [Header("Moves")]
    public PlayerMoveSetScriptableObj _MoveSet;
	
    //-->type
    public enum WeaponType
    {
        unarmed,
        sword,
        bigSword
    }
	[Header("Type")]
    public WeaponType _WeaponType;
	
    [Header("Effects")]
	public GameObject _WeaponAttackFX;
    public GameObject _WeaponHitSpark;

    [Header("Weapon Durability")]
    public bool infiniteDurability;
    public int _Durability;
    public int _DurabilityAttackCost;
    public int _DurabilitySpecialCost;

    [field:SerializeField, Header("Elemental Damage")]
    public WeaponElementalProperties _WeaponElementalProps;
}

[Serializable]
public class WeaponElementalProperties
{
	[SerializeField]
	public float fireDamage;
	[SerializeField]
	public float iceDamage;
	[SerializeField]
	public float plasmaDamage;
}