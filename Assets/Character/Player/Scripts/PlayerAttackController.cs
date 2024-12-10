using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : CharacterAttackController
{
    private PlayerCharacter player;

    [field: SerializeField, Header("Basic Attack States")] public PlayerAttackSuperState _AttackA { get; private set; }
    [field: SerializeField] public PlayerAttackSuperState _AttackB { get; private set; }
    [field: SerializeField] public PlayerAttackSuperState _AttackC { get; private set; }

    [field: SerializeField, Header("Special Attack States")] public PlayerSpecialSuperState _Special { get; private set; }
    [field: SerializeField] public PlayerSpecialSuperState _FinisherA { get; private set; }
    [field: SerializeField] public PlayerSpecialSuperState _FinisherB { get; private set; }
    [field: SerializeField] public PlayerSpecialSuperState _FinisherC { get; private set; }

    [field: SerializeField, Header("Universal Attack States")] public PlayerRushAttackSuperState _RushAttack { get; private set; }
    [field: SerializeField] public PlayerLauncherAttackSuperState _LauncherAttack { get; private set; }

    [field: SerializeField, Header("Air Attacks")] public PlayerAttackSuperState _AirSpecial { get; private set; }
    [field: SerializeField] public PlayerAttackSuperState _AirAttackA { get; private set; }
    [field: SerializeField] public PlayerAttackSuperState _AirAttackB { get; private set; }
    [field: SerializeField] public PlayerAttackSuperState _AirAttackC { get; private set; }

    [field: SerializeField, Header("Defensive Action")] public PlayerDefenseSuperState _DefenseAction { get; private set; }

    

    // delete later
    private void Update()
    {
    }
    // delete later

    public override void Initialize(Character character)
    {
        base.Initialize(character);

        player = character as PlayerCharacter;
    }

    public void LoadMoveset(MoveSetScriptableObject moves)
    {
        player._AnimationController.SetBool(player._WeaponController._CurrentWeapon._WeaponType.ToString(), true); // set our weapon type
        _AttackA = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._AttackA);
        _AttackA.InitializeState(player, "attackA", player._StateMachine);

        LoadAttackStrings();
        LoadSpecialAttacks();
        LoadUniversalAttacks();
        LoadAirAttacks();

        _DefenseAction = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._Defense);
        _DefenseAction.InitializeState(player, "defense", player._StateMachine);
    }

    private void LoadAttackStrings()
    {
        // basic attacks
        if (player._WeaponController._CurrentWeapon._MoveSet._AttackB)
        {
            _AttackB = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._AttackB);
            _AttackB.InitializeState(player, "attackB", player._StateMachine);
        }
        if (player._WeaponController._CurrentWeapon._MoveSet._AttackC)
        {
            _AttackC = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._AttackC);
            _AttackC.InitializeState(player, "attackC", player._StateMachine);
        }
    }
    
    private void LoadSpecialAttacks()
    {
        if (player._WeaponController._CurrentWeapon._MoveSet._Special)
        {
            _Special = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._Special);
            _Special.InitializeState(player, "special", player._StateMachine);
        }
        if (player._WeaponController._CurrentWeapon._MoveSet._SpecialFinisherA)
        {
            _FinisherA = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._SpecialFinisherA);
            _FinisherA.InitializeState(player, "finisherA", player._StateMachine);
        }
        if (player._WeaponController._CurrentWeapon._MoveSet._SpecialFinisherB)
        {
            _FinisherB = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._SpecialFinisherB);
            _FinisherB.InitializeState(player, "finisherB", player._StateMachine);
        }
        if (player._WeaponController._CurrentWeapon._MoveSet._SpecialFinisherC)
        {
            _FinisherC = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._SpecialFinisherC);
            _FinisherC.InitializeState(player, "finisherC", player._StateMachine);
        }
    }

    private void LoadUniversalAttacks()
    {
        if (player._WeaponController._CurrentWeapon._MoveSet._RushAttack)
        {
            _RushAttack = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._RushAttack);
            _RushAttack.InitializeState(player, "rush", player._StateMachine);
        }
        if (player._WeaponController._CurrentWeapon._MoveSet._LauncherAttack)
        {
            _LauncherAttack = Instantiate(player._WeaponController._CurrentWeapon._MoveSet._LauncherAttack);
            _LauncherAttack.InitializeState(player, "launcher", player._StateMachine);
        }
    }

    private void LoadAirAttacks()
    {
        if (player._WeaponController._CurrentWeapon._MoveSet._AirSpecial)
        {
            _AirSpecial = player._WeaponController._CurrentWeapon._MoveSet._AirSpecial;
            _AirSpecial.InitializeState(player, "airSpecial", player._StateMachine);
        }
        if (player._WeaponController._CurrentWeapon._MoveSet._AirAttackA)
        {
            _AirAttackA = player._WeaponController._CurrentWeapon._MoveSet._AirAttackA;
            _AirAttackA.InitializeState(player, "airA", player._StateMachine);
        }
        if (player._WeaponController._CurrentWeapon._MoveSet._AirAttackB)
        {
            _AirAttackB = player._WeaponController._CurrentWeapon._MoveSet._AirAttackB;
            _AirAttackB.InitializeState(player, "airB", player._StateMachine);
        }
        if (player._WeaponController._CurrentWeapon._MoveSet._AirAttackC)    
        {
            _AirAttackC = player._WeaponController._CurrentWeapon._MoveSet._AirAttackC;
            _AirAttackC.InitializeState(player, "airC", player._StateMachine);
        }
    }

    public void UnloadMoveSet()
    {
        player._AnimationController.SetBool(player._WeaponController._CurrentWeapon._WeaponType.ToString(), false); // reset our weapon type
        // attacks
        if (_AttackA != null) Destroy(_AttackA);
        if (_AttackB != null) Destroy(_AttackB);
        if (_AttackC != null) Destroy(_AttackC);
        // specials
        if (_Special != null) Destroy(_Special);
        if (_FinisherA != null) Destroy(_FinisherA);
        if (_FinisherB != null) Destroy(_FinisherB);
        if (_FinisherC != null) Destroy(_FinisherC);
        // universals
        Destroy(_DefenseAction);
    }

    public override void SetAttackParameters(bool knockback, bool launch, int damageModifier = 0)
    {
        _Damage = player._WeaponController._CurrentWeapon._Dmg + damageModifier;
        base.SetAttackParameters(knockback, launch, damageModifier);
    }

	public override void PlayHitSpark(Vector3 hitPos)
	{
		base.PlayHitSpark(hitPos);

        Instantiate(player._WeaponController._CurrentWeapon._WeaponHitSpark, hitPos, Quaternion.identity);
	}

	// end
}
