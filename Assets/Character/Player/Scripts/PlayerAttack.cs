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
    [field: SerializeField] public GameObject _WeaponL { get; private set; }
    [field: SerializeField] public Transform _WeaponHolderR { get; private set; }
    [field: SerializeField] public GameObject _WeaponR { get; private set; }

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
        List<Transform> targetList = GetAllDamageableEntities();
        Transform target = null;
        foreach (Transform obj in targetList)
        {
            if (target == null) target = obj;
            else if (Vector3.Distance(transform.position, obj.position) < Vector3.Distance(transform.position, target.position)) target = obj;
        }
        TurnToFaceTarget(target);
    }

    private void TurnToFaceTarget(Transform target)
    {
        if (target == null) return;
        transform.LookAt(target);
    }

    private List<Transform> GetAllDamageableEntities()
    {
        List<Transform> entities = new List<Transform>();
        Vector3 startPos = player._PlayerActor.transform.position;
        float lineLength = 5.0f;

        // Below is how to calculate things in front
        RaycastHit forwardHit;
        Vector3 forwardLineDirection = (startPos + player._PlayerActor.transform.forward * lineLength);
        forwardLineDirection.y = 1.0f;
        if(Physics.Linecast(startPos, forwardLineDirection, out forwardHit))
        {
            IDamageable damageableEntity = forwardHit.collider.GetComponent<IDamageable>();
            if ( damageableEntity != null)
            {
                if (!entities.Contains(forwardHit.transform) && damageableEntity.GetControllingEntity() != player) entities.Add(forwardHit.transform);
            }
        }

        // below is for the right of the player
        RaycastHit rightHit;
        Vector3 rightLineDirection = (startPos + new Vector3(player._PlayerActor.transform.forward.x + 0.35f, player._PlayerActor.transform.forward.y, player._PlayerActor.transform.forward.z) * lineLength);
        if (Physics.Linecast(startPos, rightLineDirection, out rightHit))
        {
            IDamageable damageableEntity = rightHit.collider.GetComponent<IDamageable>();
            if (damageableEntity != null)
            {
                if (!entities.Contains(rightHit.transform) && damageableEntity.GetControllingEntity() != player) entities.Add(rightHit.transform);
            }
        }
        // and far right
        RaycastHit farRightHit;
        Vector3 farRightLineDirection = (startPos + new Vector3(player._PlayerActor.transform.forward.x + 0.75f, player._PlayerActor.transform.forward.y, player._PlayerActor.transform.forward.z) * lineLength);
        if (Physics.Linecast(startPos, farRightLineDirection, out farRightHit))
        {
            IDamageable damageableEntity = farRightHit.collider.GetComponent<IDamageable>();
            if (damageableEntity != null)
            {
                if (!entities.Contains(farRightHit.transform) && damageableEntity.GetControllingEntity() != player) entities.Add(farRightHit.transform);
            }
        }

        // now for the left direction
        RaycastHit leftHit;
        Vector3 leftLineDirection = (startPos + new Vector3(player._PlayerActor.transform.forward.x - 0.35f, player._PlayerActor.transform.forward.y, player._PlayerActor.transform.forward.z) * lineLength);
        if (Physics.Linecast(startPos, leftLineDirection, out leftHit))
        {
            IDamageable damageableEntity = leftHit.collider.GetComponent<IDamageable>();
            if (damageableEntity != null)
            {
                if (!entities.Contains(leftHit.transform) && damageableEntity.GetControllingEntity() != player) entities.Add(leftHit.transform);
            }
        }
        // and the far-left
        RaycastHit farLeftHit;
        Vector3 farLeftLineDirection = (startPos + new Vector3(player._PlayerActor.transform.forward.x - 0.75f, player._PlayerActor.transform.forward.y, player._PlayerActor.transform.forward.z) * lineLength);
        if (Physics.Linecast(startPos, farLeftLineDirection, out farLeftHit))
        {
            IDamageable damageableEntity = farLeftHit.collider.GetComponent<IDamageable>();
            if (damageableEntity != null)
            {
                if (!entities.Contains(farLeftHit.transform) && damageableEntity.GetControllingEntity() != player) entities.Add(farLeftHit.transform);
            }
        }

        // draw all the stuff
        DrawTargetDetectionLines(1.5f, startPos, forwardLineDirection, rightLineDirection, farRightLineDirection, leftLineDirection, farLeftLineDirection);

        return entities;
    }

    private void DrawTargetDetectionLines(float timeToShow, Vector3 start, Vector3 forward, Vector3 right, Vector3 farRight, Vector3 left, Vector3 farLeft)
    {
        Debug.DrawLine(start, forward, Color.white, timeToShow);
        Debug.DrawLine(start, right, Color.cyan, timeToShow);
        Debug.DrawLine(start, farRight, Color.blue, timeToShow);
        Debug.DrawLine(start, left, Color.magenta, timeToShow);
        Debug.DrawLine(start, farLeft, Color.red, timeToShow);
    }

    // end
}
