using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSpell : MonoBehaviour
{
    private PlayerCharacter player;

    [field: SerializeField] public SpellCardScriptableObj _baseSpell { get; private set; }

    [field: SerializeField] public Transform _spellTarget { get; private set; }

    private float timeToAddToTimer;
    [SerializeField] private float _spellTimer;

    [Header("UI")]
    [Tooltip("The UI Controller")]
    [SerializeField]
    private PlayerUIController _UI;

	private Ray targetDetectRay = new Ray();

	public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        //cam = Camera.main;
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
        }
    }

    private void ShootSpell(Vector3 targetPos)
    {
        player._AnimationController.SetTrigger(_UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellAnimationBool.ToString());
        player._Aether.UseAether(_UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellAetherCost);
        
        Projectile conjuredSpell = ObjectPoolManager.GetObjectFromPool(_UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellProjectile, _spellTarget.transform.position, Quaternion.identity, ObjectPoolManager.PoolFolder.Projectile).GetComponent<Projectile>();
		if (targetPos !=  Vector3.zero)
        {
			Quaternion targetDir = Quaternion.Euler(player._PlayerActor.transform.position - targetPos);
            conjuredSpell.transform.eulerAngles = _spellTarget.transform.eulerAngles;
		}
        else
        {
            print($"no target pos");
			Vector3 targetDir = targetPos - _spellTarget.transform.position;
            conjuredSpell.transform.eulerAngles = targetDir;
        }
        conjuredSpell.InitializeProjectile(player, _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellDamage, _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellProjectileSpeed, _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellLifetime, _UI._ActiveSpellList[_UI._CurrentSpellIndex].spell._SpellImpactVFX);
        _spellTimer = timeToAddToTimer;
        _UI.RemoveSpellCharge();
    }

    public Vector3 GetIntendedTarget()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        Ray ray = player._CameraController._Camera.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            //print(hit.transform.name);
            return hit.point;
        }
        return Vector3.zero;
    }

    public void RotateSpellTarget()
    {
        //Quaternion rotation = cam.transform.rotation;
        Vector3 rotation = player._CameraController._Camera.transform.eulerAngles;
        //print($"{rotation.x}, {rotation.y}, {rotation.z}, {rotation.w}");
        _spellTarget.eulerAngles = rotation;
        //Quaternion.Lerp(_spellTarget.transform.rotation, rotation, 2.0f);
    }
    public void ResetSpellTargetRotation() => _spellTarget.rotation = new Quaternion(0, 0, 0, 0);

    // end
}
