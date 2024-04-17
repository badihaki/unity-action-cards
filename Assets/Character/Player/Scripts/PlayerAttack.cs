using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerCharacter player;
    [SerializeField] private UnarmedDefaultWeapon unarmed;
    [field: SerializeField] public UnarmedDefaultWeapon _CurrentWeapon { get; private set; }

    [field: SerializeField] public PlayerAttackSuperState _AttackA { get; private set; }
    [field: SerializeField] public PlayerAttackSuperState _AttackB { get; private set; }
    [field: SerializeField] public PlayerAttackSuperState _AttackC { get; private set; }

    [field: Header("Attack Stats"), SerializeField]
    public int _Damage { get; private set; }
    [field: SerializeField] public float _KnockbackForce { get; private set; }
    [field: SerializeField] public float _LaunchForce { get; private set; }

    [field: Header("Where the weapons lie"), SerializeField]
    public Transform _WeaponHolderL { get; private set; }
    [field: SerializeField] public GameObject _WeaponL;
    [field: SerializeField] public Transform _WeaponHolderR { get; private set; }
    [field: SerializeField] public GameObject _WeaponR;

    public void Initialize(PlayerCharacter newPlayer)
    {
        player = newPlayer;
        SetWeapon(unarmed);
    }

    public void SwitchWeapon(UnarmedDefaultWeapon weapon)
    {
        player._AnimationController.SetBool(_CurrentWeapon._WeaponType.ToString(), false);
        DestroyWeaponGameObjects();
        SetWeapon(weapon);
    }

    private void DestroyWeaponGameObjects()
    {
        if (_WeaponL)
        {
            Destroy(_WeaponL);
            _WeaponL = null;
        }
        if(_WeaponR)
        {
            Destroy(_WeaponR);
            _WeaponR = null;
        }
    }

    private void SetWeapon(UnarmedDefaultWeapon newWeapon)
    {
        _CurrentWeapon = newWeapon;
        LoadWeaponGameObjects(_CurrentWeapon._WeaponGameObjectL, _CurrentWeapon._WeaponGameObjectR);
        LoadMoveset();
        player._AnimationController.SetBool(_CurrentWeapon._WeaponType.ToString(), true);
    }

    private void LoadWeaponGameObjects(GameObject weaponL = null, GameObject weaponR = null)
    {
        if (weaponL != null)
        {
            _WeaponL = Instantiate(weaponL, _WeaponHolderL);
        }
        else print("no left-hand weapon");
        if(weaponR != null)
        {
            _WeaponR = Instantiate(weaponR, _WeaponHolderR);
        }
        else print("no right-hand weapon");
    }

    private void LoadMoveset()
    {
        _AttackA = Instantiate(_CurrentWeapon._PlayerMoves._AttackA);
        _AttackA.InitializeState(player, "attack", player._StateMachine);
        
        _AttackB = Instantiate(_CurrentWeapon._PlayerMoves._AttackB);
        _AttackB.InitializeState(player, "attack", player._StateMachine);

        _AttackC = Instantiate(_CurrentWeapon._PlayerMoves._AttackC);
        _AttackC.InitializeState(player, "attack", player._StateMachine);
    }

    public void SetAttackParameters(int damage, float knockbackForce, float launchForce)
    {
        _Damage = damage;
        _KnockbackForce = knockbackForce;
        _LaunchForce = launchForce;
    }

    public void ResetAttackParameters()
    {
        _Damage = 0;
        _KnockbackForce = 0.0f;
        _LaunchForce = 0.0f;
    }

    // end
}
