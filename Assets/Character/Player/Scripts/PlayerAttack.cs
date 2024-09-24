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

    public Vector3 DetectNearbyTargets()
    {
        List<Transform> targetList = GetAllDamageableEntities();
        Transform target = null;
        foreach (Transform obj in targetList)
        {
            if (target == null) target = obj;
            else if (Vector3.Distance(transform.position, obj.position) < Vector3.Distance(transform.position, target.position)) target = obj;
        }
        if(target != null)
        {
            TurnToFaceTarget(target);
            return target.transform.position;
        }
        return Vector3.zero;
    }

    private void TurnToFaceTarget(Transform target)
    {
        if (target == null) return;
        Vector3 lookatTarget = new Vector3(target.position.x, player._PlayerActor.transform.position.y, target.position.z);
        // transform.LookAt(target);
        // player._LocomotionController.RotateCharacter(target.transform.position - target.position);
        player._PlayerActor.transform.LookAt(lookatTarget);
    }

    private List<Transform> GetAllDamageableEntities()
    {
        List<Transform> entities = new List<Transform>();
        // Vector3 startPos = player._PlayerActor.transform.position;
        // Vector3 startPos = player._PlayerSpells._spellTarget.transform.position;
        Vector3 startPos = new Vector3(player._PlayerActor.transform.position.x, player._PlayerActor.transform.position.y + 0.75f, player._PlayerActor.transform.position.z);
        float lineLength = 12.35f;
        
        ShootRays(5, 7, lineLength, startPos, 0.5f);
       
        /*
        // Below is how to calculate things in front
        RaycastHit forwardHit;

        // Vector3 forwardLineDirection = (startPos + player._PlayerActor.transform.forward * lineLength);
        Vector3 forwardLineDirection = (startPos + player._CameraRef.transform.forward * lineLength);
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
        Vector3 rightLineDirection = (startPos + new Vector3(player._CameraRef.transform.forward.x + 0.35f, player._CameraRef.transform.forward.y, player._CameraRef.transform.forward.z) * lineLength);
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
        Vector3 farRightLineDirection = (startPos + new Vector3(player._CameraRef.transform.forward.x + 0.75f, player._CameraRef.transform.forward.y, player._CameraRef.transform.forward.z) * lineLength);
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
        Vector3 leftLineDirection = (startPos + new Vector3(player._CameraRef.transform.forward.x - 0.35f, player._CameraRef.transform.forward.y, player._CameraRef.transform.forward.z) * lineLength);
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
        Vector3 farLeftLineDirection = (startPos + new Vector3(player._CameraRef.transform.forward.x - 0.75f, player._CameraRef.transform.forward.y, player._CameraRef.transform.forward.z) * lineLength);
        if (Physics.Linecast(startPos, farLeftLineDirection, out farLeftHit))
        {
            IDamageable damageableEntity = farLeftHit.collider.GetComponent<IDamageable>();
            if (damageableEntity != null)
            {
                if (!entities.Contains(farLeftHit.transform) && damageableEntity.GetControllingEntity() != player) entities.Add(farLeftHit.transform);
            }
        }
        // draw all the stuff
        DrawTargetDetectionLines(0.5f, startPos, forwardLineDirection, rightLineDirection, farRightLineDirection, leftLineDirection, farLeftLineDirection);
        */
        return entities;
    }

    private void ShootRays(int columns, int rows, float rayLength, Vector3 startingPos, float lineDrawTime)
    {
        List<Transform> entities = new List<Transform>();

        Vector2 offset = new Vector2(-20,-10);
        Color color = Color.red;
        //
        for (int iRow = 0; iRow < rows; iRow++)
        {
            // for each row, do...
            for (int iCol = 0; iCol < columns; iCol++)
            {
                print($"row {iRow+1} and column {iCol+1}");
                print($"offset {offset.x * 0.1f} - x || {offset.y * 1.0f} - y");
                // create a ray, add offset
                RaycastHit hit;
                // Vector3 dir = (startingPos + new Vector3(player._CameraRef.transform.forward.x + offset.x * 0.1f, player._CameraRef.transform.forward.y, player._CameraRef.transform.forward.z) * rayLength);
                Vector3 dir = (startingPos + new Vector3(player._CameraRef.transform.forward.x + offset.x * 0.1f, offset.y, player._CameraRef.transform.forward.z) * rayLength);
                if(Physics.Linecast(startingPos, dir, out hit))
                {
                    if (!entities.Contains(hit.transform)) entities.Add(hit.transform);
                }
                Debug.DrawLine(startingPos, dir, color, lineDrawTime);
                offset.x += 5;
            }
            // print(color.r);
            color.r += -0.25f;
            color.b += 0.35f;
            color.g += -0.15f;
            offset.y += 5;
        }
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
