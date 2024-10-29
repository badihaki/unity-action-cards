using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Card/New Weapon Card")]
public class WeaponCardScriptableObject : CardScriptableObj
{
    [Header("Weapon Scriptable Obj")]
    public WeaponScriptableObj _Weapon;

    protected override void UseCardAbility(Character controllingCharacter)
    {
        if (controllingCharacter.GetComponent<PlayerCharacter>() != null)
        {
            PlayerCharacter player = controllingCharacter.GetComponent<PlayerCharacter>();
            player._WeaponController.SwitchWeapon(_Weapon);
        }
    }
}
