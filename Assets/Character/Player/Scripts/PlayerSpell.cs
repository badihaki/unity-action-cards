using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSpell : MonoBehaviour
{
    private PlayerCharacter player;
    private Camera cam;

    [field: SerializeField] public SpellCardScriptableObj _baseSpell { get; private set; }

    [field: SerializeField] public Transform _spellTarget { get; private set; }

    private float timeToAddToTimer;
    [SerializeField] private float _spellTimer;

    [Header("UI")]
    [Tooltip("The UI Controller")]
    [SerializeField]
    private PlayerUIController _UI;

	private Ray targetDetectRay = new Ray();
    [field: SerializeField]
    private List<Actor> targetableActors = new List<Actor>();


	public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        cam = Camera.main;
        _UI = GetComponent<PlayerUIController>();
        _spellTarget = player._PlayerActor.transform.Find("SpellTarget");
        _spellTimer = 0.0f;
    }

    private void Update()
    {
        RunSpellTimer();
    }

    private void RunSpellTimer()
    {
        if (_spellTimer <= 0) _spellTimer = 0.0f;
        else
        {
            _spellTimer -= Time.deltaTime;
        }
    }

    public void ResetSpellTimer() => _spellTimer = 0.0f;
    public void SetHowMuchTimeToAddToSpellTimer(float timeInFloat) => timeToAddToTimer = timeInFloat;


    public void UseSpell(Vector3 targetPos)
    {
        
        if (_spellTimer <= 0 && player._Aether._CurrentAether > _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellAetherCost)
        {
            ShootSpell(targetPos);
            if (GameManagerMaster.GameMaster.logExraPlayerData)
            {
                print(">>>>>>>>> targetables");
                targetableActors.ForEach(actor =>
                {
                    print(actor.transform.parent.name);
                });
                print("/>>>>>>>>> targetables/");
            }
			targetableActors.Clear();
        }
    }

    private void ShootSpell(Vector3 targetPos)
    {
        player._AnimationController.SetTrigger(_UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellAnimationBool.ToString());
        player._Aether.UseAether(_UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellAetherCost);
        
        Projectile conjuredSpell;
        if(targetPos !=  Vector3.zero)
        {
            //player._LocomotionController.RotateTowardsTarget(targetPos);
            conjuredSpell = Instantiate(_UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellProjectile, _spellTarget.transform.position, Quaternion.identity).GetComponent<Projectile>();
            Quaternion targetDir = Quaternion.Euler(player._PlayerActor.transform.position - targetPos);
        }
        else
        {
            conjuredSpell = Instantiate(_UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellProjectile, _spellTarget.transform.position, Quaternion.identity).GetComponent<Projectile>();
        }
		//conjuredSpell.transform.rotation = player._PlayerActor.transform.rotation;
		conjuredSpell.transform.eulerAngles = _spellTarget.transform.eulerAngles;
		conjuredSpell.name = _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._CardName;
        conjuredSpell.InitializeProjectile(player, _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellDamage, _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellProjectileSpeed, _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellLifetime, _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellImpactVFX);
        _spellTimer = timeToAddToTimer;
        _UI.RemoveSpellCharge();
    }

    public Vector3 GetIntendedTarget()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        Ray ray = cam.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public Vector3 DetectRangedTargets()
    {
        Vector3 targetPos = Vector3.zero;
		Collider[] possibleTargets = Physics.OverlapSphere(_spellTarget.position, 10.5f, LayerMask.GetMask("Character"), QueryTriggerInteraction.UseGlobal);

        if(possibleTargets.Length > 0)
        {
            foreach (var target in possibleTargets)
            {
                Actor actor = target.GetComponentInParent<Actor>();
                if (actor != null && actor != player._Actor)
                {
                    if (!targetableActors.Contains(actor))
                        targetableActors.Add(actor);
                }
            }

            if(targetableActors.Count > 0)
            {
                foreach (var actor in targetableActors)
                {
                    if (targetPos == Vector3.zero)
                        targetPos = actor.transform.position;
                    else if(Vector3.Distance(targetPos, player._PlayerActor.transform.position) > Vector3.Distance(targetPos, actor.transform.position)) // if distance between current target pos and player actor is greater than distance between target pos and target actor
                    {
                        targetPos = actor.transform.position;
                    }    
                }
            }
        }
        print($"target position is {targetPos.ToString()}");
        return targetPos;
    }

    public void RotateSpellTarget()
    {
        //Quaternion rotation = cam.transform.rotation;
        Vector3 rotation = cam.transform.eulerAngles;
        //print($"{rotation.x}, {rotation.y}, {rotation.z}, {rotation.w}");
        _spellTarget.eulerAngles = rotation;
        //Quaternion.Lerp(_spellTarget.transform.rotation, rotation, 2.0f);
    }
    public void ResetSpellTargetRotation() => _spellTarget.rotation = new Quaternion(0, 0, 0, 0);

    // end
}
