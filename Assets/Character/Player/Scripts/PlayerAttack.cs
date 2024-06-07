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

    [field: SerializeField, Header("Defensive Action")] public PlayerState _DefenseAction { get; private set; }

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

    public void SwitchWeapon(WeaponScriptableObj weapon)
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

    private void SetWeapon(WeaponScriptableObj newWeapon)
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
        _AttackA.InitializeState(player, "attackA", player._StateMachine);
        
        _AttackB = Instantiate(_CurrentWeapon._PlayerMoves._AttackB);
        _AttackB.InitializeState(player, "attackB", player._StateMachine);

        _AttackC = Instantiate(_CurrentWeapon._PlayerMoves._AttackC);
        _AttackC.InitializeState(player, "attackC", player._StateMachine);

        _DefenseAction = Instantiate(_CurrentWeapon._PlayerMoves._DefenseAction);
        _DefenseAction.InitializeState(player, "defense", player._StateMachine);
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

    public void DetectNearbyTargets()
    {
        print("searching for targets");
        /*
         * Need to create a collider
         * W/ collider, lets detect anything that's IDamageable
         * Determine the closest IDamageable
         * Face that enemy
         */
        var allDamageableEntities = GetAllDamageableEntities();
        print(allDamageableEntities.Count);
    }

    private List<Transform> GetAllDamageableEntities()
    {
        List<Transform> entities = new List<Transform>();

        float lineLength = 3.0f;
        // Vector3 forwardLineDirection = new Vector3(player._PlayerActor.transform.forward.x, 0, player._PlayerActor.transform.forward.z).normalized;
        // Vector3 forwardLineDirection = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized * lineLength;
        Vector3 forwardLineDirection = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.forward.z * lineLength);


        Debug.DrawLine(player._PlayerActor.transform.position, forwardLineDirection * lineLength, Color.red, 1.5f);
        if(Physics.Linecast(player._PlayerActor.transform.position, forwardLineDirection, out RaycastHit hitInfo))
        {
            if(hitInfo.transform.gameObject.GetComponent<IDamageable>() != null)
            {
                entities.Add(hitInfo.transform);
            }
        }

        return entities;
    }

    // end
}
