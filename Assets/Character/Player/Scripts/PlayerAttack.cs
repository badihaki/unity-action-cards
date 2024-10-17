using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerCharacter player;
    [SerializeField] private WeaponScriptableObj unarmed;
    [field: SerializeField] public WeaponScriptableObj _CurrentWeapon { get; private set; }

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

    [field: Header("Attack Stats"), SerializeField]
    public int _Damage { get; private set; }
    [field: SerializeField] public float _KnockbackForce { get; private set; }
    [field: SerializeField] public float _LaunchForce { get; private set; }

    [field: Header("Where the weapons lie"), SerializeField]
    public Transform _WeaponHolderL { get; private set; }
    [field: SerializeField] public GameObject _WeaponL { get; private set; }
    [field: SerializeField] public Transform _WeaponHolderR { get; private set; }
    [field: SerializeField] public GameObject _WeaponR { get; private set; }

    [Header("Ray stuff, delete later")]
    public Vector2 offset = new Vector2(0, 0);
    public float offsetAddX = 0.0f;
    public float offsetAddY = 0.5f;
    public Vector3 startPos;
    public float lineLength = 12.35f;
    public float rayStartOffsetY = 1.50f;
    public Vector3 camForward;
    // public List<Color> colors = [Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white];
    public List<Color> colors = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white };

    // delete later
    private void Update()
    {
        // startPos = new Vector3(player._PlayerActor.transform.position.x, player._PlayerActor.transform.position.y + rayStartOffsetY, player._PlayerActor.transform.position.z);
        // ShootRays(5, 7, lineLength, startPos, 0.085f);
    }
    // delete later

    public void Initialize(PlayerCharacter newPlayer)
    {
        player = newPlayer;
        _WeaponHolderR = player._PlayerActor.RightWeapon;
        _WeaponHolderL = player._PlayerActor.LeftWeapon;
        _WeaponR = null;
        _WeaponL = null;
        SetWeapon(unarmed);
        _WeaponR.gameObject.SetActive(false);

    }

    public void SwitchWeapon(WeaponScriptableObj weapon)
    {
        player._AnimationController.SetBool(_CurrentWeapon._WeaponType.ToString(), false);
        DestroyWeaponGameObjects();
        UnloadMoveSet();
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

    private void SetWeapon(WeaponScriptableObj newWeapon)
    {
        _CurrentWeapon = newWeapon;
        LoadWeaponGameObjects(_CurrentWeapon._WeaponGameObjectL, _CurrentWeapon._WeaponGameObjectR);
        LoadMoveset();
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
        player._AnimationController.SetBool(_CurrentWeapon._WeaponType.ToString(), true); // set our weapon type
        _AttackA = Instantiate(_CurrentWeapon._PlayerMoves._AttackA);
        _AttackA.InitializeState(player, "attackA", player._StateMachine);

        LoadAttackStrings();
        LoadSpecialAttacks();
        LoadUniversalAttacks();
        LoadAirAttacks();

        _DefenseAction = Instantiate(_CurrentWeapon._PlayerMoves._Defense);
        _DefenseAction.InitializeState(player, "defense", player._StateMachine);
    }

    private void LoadAttackStrings()
    {
        // basic attacks
        if (_CurrentWeapon._PlayerMoves._AttackB)
        {
            _AttackB = Instantiate(_CurrentWeapon._PlayerMoves._AttackB);
            _AttackB.InitializeState(player, "attackB", player._StateMachine);
        }
        if (_CurrentWeapon._PlayerMoves._AttackC)
        {
            _AttackC = Instantiate(_CurrentWeapon._PlayerMoves._AttackC);
            _AttackC.InitializeState(player, "attackC", player._StateMachine);
        }
    }
    
    private void LoadSpecialAttacks()
    {
        if (_CurrentWeapon._PlayerMoves._Special)
        {
            _Special = Instantiate(_CurrentWeapon._PlayerMoves._Special);
            _Special.InitializeState(player, "special", player._StateMachine);
        }
        if (_CurrentWeapon._PlayerMoves._SpecialFinisherA)
        {
            _FinisherA = Instantiate(_CurrentWeapon._PlayerMoves._SpecialFinisherA);
            _FinisherA.InitializeState(player, "finisherA", player._StateMachine);
        }
        if (_CurrentWeapon._PlayerMoves._SpecialFinisherB)
        {
            _FinisherB = Instantiate(_CurrentWeapon._PlayerMoves._SpecialFinisherB);
            _FinisherB.InitializeState(player, "finisherB", player._StateMachine);
        }
        if (_CurrentWeapon._PlayerMoves._SpecialFinisherC)
        {
            _FinisherC = Instantiate(_CurrentWeapon._PlayerMoves._SpecialFinisherC);
            _FinisherC.InitializeState(player, "finisherC", player._StateMachine);
        }
    }

    private void LoadUniversalAttacks()
    {
        if (_CurrentWeapon._PlayerMoves._RushAttack)
        {
            _RushAttack = Instantiate(_CurrentWeapon._PlayerMoves._RushAttack);
            _RushAttack.InitializeState(player, "rush", player._StateMachine);
        }
        if (_CurrentWeapon._PlayerMoves._LauncherAttack)
        {
            _LauncherAttack = Instantiate(_CurrentWeapon._PlayerMoves._LauncherAttack);
            _LauncherAttack.InitializeState(player, "launcher", player._StateMachine);
        }
    }

    private void LoadAirAttacks()
    {
        if (_CurrentWeapon._PlayerMoves._AirSpecial)
        {
            _AirSpecial = _CurrentWeapon._PlayerMoves._AirSpecial;
            _AirSpecial.InitializeState(player, "airSpecial", player._StateMachine);
        }
        if (_CurrentWeapon._PlayerMoves._AirAttackA)
        {
            _AirAttackA = _CurrentWeapon._PlayerMoves._AirAttackA;
            _AirAttackA.InitializeState(player, "airA", player._StateMachine);
        }
        if (_CurrentWeapon._PlayerMoves._AirAttackB)
        {
            _AirAttackB = _CurrentWeapon._PlayerMoves._AirAttackB;
            _AirAttackB.InitializeState(player, "airB", player._StateMachine);
        }
        if (_CurrentWeapon._PlayerMoves._AirAttackC)
        {
            _AirAttackC = _CurrentWeapon._PlayerMoves._AirAttackC;
            _AirAttackC.InitializeState(player, "airC", player._StateMachine);
        }
    }

    private void UnloadMoveSet()
    {
        player._AnimationController.SetBool(_CurrentWeapon._WeaponType.ToString(), false); // reset our weapon type
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

    public void SetRootMotion(bool value)
    {
        player._PlayerActor.animationController.applyRootMotion = value;
        print($"we have root motion?? {player._PlayerActor.animationController.hasRootMotion}");
    }

    public void DetectMeleeTargets()
    {
        Vector3 targetPos = Vector3.zero;
        Vector2 moveInput = player._Controls._MoveInput;
        // print($"number of targets {player._LockOnTargeter.rangeTargets.Count}");
        if (player._LockOnTargeter.meleeTargets.Count > 0)
        {
            player._LockOnTargeter.rangeTargets.ForEach(t =>
            {
                print($">>>>>> targetable object is {t.name} with a position of {t.position} || playerSpell.DetectRangeTargets");
                if (targetPos == Vector3.zero) targetPos = t.position;
                else if (Vector3.Distance(transform.position, t.position) > Vector3.Distance(transform.position, targetPos))
                {
                    targetPos = t.position;
                }
            });

            // player._LocomotionController.RotateTowardsTarget(targetPos);
        }
        print($"Move direction at time of attack - {moveInput}");
        if(moveInput != Vector2.zero)
            player._LocomotionController.RotateInstantly(moveInput);
    }
    // end
}
