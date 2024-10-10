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

        _DefenseAction = Instantiate(_CurrentWeapon._PlayerMoves._DefenseDash);
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
        
        
        /*
         * TODO
         * The rays do not shoot the right direction, and this needs to be fixed
         * v
         * v
         * v
        ShootRays(5, 7, lineLength, startPos, 0.5f);
         */
       
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
        print("shooting");
        
        Vector3 rayOffset = offset;
        //
        for (int iRow = 0; iRow < rows; iRow++)
        {
            // for each row, do...
            for (int iCol = 0; iCol < columns; iCol++)
            {
                // print($"row {iRow+1} and column {iCol+1}");
                // print($"offset {offset.x * 0.1f} - x || {offset.y * 1.0f} - y");
                // create a ray, add offset
                RaycastHit hit;
                // Vector3 dir = (startingPos + new Vector3(player._CameraRef.transform.forward.x + offset.x * 0.1f, player._CameraRef.transform.forward.y, player._CameraRef.transform.forward.z) * rayLength);
                // Vector3 dir = (startingPos + new Vector3(player._CameraRef.transform.forward.x + rayOffset.x, rayOffset.y, player._CameraRef.transform.forward.z * rayOffset.z) * rayLength);
                Vector3 dir = startingPos + ((player._CameraRef.transform.forward + rayOffset) * rayLength);

                // if (Physics.Linecast(startingPos, dir, out hit))
                if (Physics.Linecast(startingPos, dir, out hit))
                {
                    if (!entities.Contains(hit.transform)) entities.Add(hit.transform);
                }
                Debug.DrawLine(startingPos, dir, colors[iCol], lineDrawTime);
                rayOffset.x += offsetAddX;
            }
            rayOffset.y += offsetAddY;
        }
}

    // end
}
