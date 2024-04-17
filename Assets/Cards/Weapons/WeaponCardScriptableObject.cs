using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Card/New Weapon Card")]
public class WeaponCardScriptableObject : CardScriptableObj
{
    [Header("Weapon Game Object")]
    public UnarmedDefaultWeapon _Weapon;
    [Header("Weapon Durability Stats")]
    public float _WeaponDurability;
    public float _WeaponAttackDurabilityCost;
    public bool _NoDurabilityTimer;

    protected override void UseCardAbility(Character controllingCharacter)
    {
        if (controllingCharacter.GetComponent<PlayerCharacter>() != null)
        {
            PlayerCharacter player = controllingCharacter.GetComponent<PlayerCharacter>();
            player._AttackController.SwitchWeapon(_Weapon);
        }
    }
}
