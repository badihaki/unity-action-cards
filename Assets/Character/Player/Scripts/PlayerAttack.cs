using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerCharacter player;
    [SerializeField] private WeaponScriptableObject noWeapon;
    [field: SerializeField] public WeaponScriptableObject _CurrentWeapon { get; private set; }

    public PlayerAttackSuperState _AttackA { get; private set; }
    public PlayerAttackSuperState _AttackB { get; private set; }
    public PlayerAttackSuperState _AttackC { get; private set; }

    [field:Header("Attack Stats"),SerializeField]
    public int _Damage { get; private set; }
    public float _KnockbackForce { get; private set; }
    public float _LaunchForce { get; private set; }

    public void Initialize(PlayerCharacter newPlayer)
    {
        player = newPlayer;
        SwitchWeapons(noWeapon);
    }

    private void SwitchWeapons(WeaponScriptableObject newWeapon)
    {
        _CurrentWeapon = newWeapon;
        LoadMoveset();
    }

    private void LoadMoveset()
    {
        _AttackA = Instantiate(_CurrentWeapon._PlayerMoves._AttackA);
        _AttackA.ManualSetUp(player, "attackA", player._StateMachine);
        
        _AttackB = Instantiate(_CurrentWeapon._PlayerMoves._AttackB);
        _AttackB.ManualSetUp(player, "attackB", player._StateMachine);

        _AttackC = Instantiate(_CurrentWeapon._PlayerMoves._AttackB);
        _AttackC.ManualSetUp(player, "attackC", player._StateMachine);
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
