using System.Collections;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
	private PlayerCharacter player;
	private PlayerAttackController attackController;

	[field: SerializeField, Header("Base Weapon")] private WeaponScriptableObj baseWeapon;
	[field: SerializeField, Header("Current Weapon")] public WeaponScriptableObj _CurrentWeapon { get; private set; }

	[field: Header("Weapon Transforms"), SerializeField] public Transform _WeaponHolderL { get; private set; }
	[field: SerializeField] public GameObject _WeaponL { get; private set; }
	[field: SerializeField] public Transform _WeaponHolderR { get; private set; }
	[field: SerializeField] public GameObject _WeaponR { get; private set; }
	[field: SerializeField] public Transform _VfxSpawnPos { get; private set; }

	[field: Header("Durability"), SerializeField] public int _CurrentWeaponDurability { get; private set; }
	[field: SerializeField] private bool infiniteDurability;
	private WaitForSeconds durabilityTimer = new WaitForSeconds(1);

	public delegate void ChangeWeaponDurability(int durability);
	public event ChangeWeaponDurability OnDurabilityChanged;
	
	public void Initialize(PlayerCharacter newPlayer)
	{
		player = newPlayer;
		attackController = player._AttackController as PlayerAttackController;
		_WeaponHolderR = player._PlayerActor.RightWeapon;
		_WeaponHolderL = player._PlayerActor.LeftWeapon;
		_WeaponR = null;
		_WeaponL = null;
		SetWeapon(baseWeapon);
		_WeaponR.gameObject.SetActive(false);
		_VfxSpawnPos = player._PlayerActor.transform.Find("VFXSpawnPos");
	}

	private void SetWeapon(WeaponScriptableObj newWeapon)
	{
		_CurrentWeapon = newWeapon;
		LoadWeaponGameObjects(_CurrentWeapon._WeaponGameObjectL, _CurrentWeapon._WeaponGameObjectR);
		attackController.LoadMoveset(newWeapon._MoveSet);

		infiniteDurability = _CurrentWeapon.infiniteDurability;

		if(!infiniteDurability)
		{
			_CurrentWeaponDurability = _CurrentWeapon._Durability;
			player._UIController.ActivateWeaponUI(_CurrentWeaponDurability);
			OnDurabilityChanged(_CurrentWeaponDurability);
			//print("starting dur timer coroutine");
			StartCoroutine(ManageDurabilityTimer());
		}
	}

	private void LoadWeaponGameObjects(GameObject weaponL = null, GameObject weaponR = null)
	{
		if (weaponL != null)
		{
			_WeaponL = Instantiate(weaponL, _WeaponHolderL);
		}
		if (weaponR != null)
		{
			_WeaponR = Instantiate(weaponR, _WeaponHolderR);
		}
		else Debug.LogWarning("no right-hand weapon!?");
	}

	public void SwitchWeapon(WeaponScriptableObj weapon)
	{
		player._AnimationController.SetBool(_CurrentWeapon._WeaponType.ToString(), false);
		DestroyWeaponGameObjects();
		player._Actor._Hitbox.SetElementalProperties(CharacterHitbox.HitBoxElementalProps.none, false);
		attackController.UnloadMoveSet();
		SetWeapon(weapon);
		SetWeaponElementalProperties(weapon);
	}

	private void SetWeaponElementalProperties(WeaponScriptableObj weapon)
	{
		if (weapon._WeaponElementalProps.fireDamage > 0)
			player._Actor._Hitbox.SetElementalProperties(CharacterHitbox.HitBoxElementalProps.fire, true, weapon._WeaponElementalProps.fireDamage);
		if (weapon._WeaponElementalProps.iceDamage > 0)
			player._Actor._Hitbox.SetElementalProperties(CharacterHitbox.HitBoxElementalProps.ice, true, weapon._WeaponElementalProps.iceDamage);
		if (weapon._WeaponElementalProps.plasmaDamage > 0)
			player._Actor._Hitbox.SetElementalProperties(CharacterHitbox.HitBoxElementalProps.plasma, true, weapon._WeaponElementalProps.plasmaDamage);
	}

	private void DestroyWeaponGameObjects()
	{
		if (_WeaponL)
		{
			Destroy(_WeaponL);
			_WeaponL = null;
		}
		if (_WeaponR)
		{
			Destroy(_WeaponR);
			_WeaponR = null;
		}
	}

	private IEnumerator ManageDurabilityTimer()
	{
		//print("raising durability timer");
		while(_CurrentWeaponDurability > 0)
		{
			yield return durabilityTimer;
			_CurrentWeaponDurability--;
			OnDurabilityChanged(_CurrentWeaponDurability);
			yield return null;
		}
		player._UIController.DisableWeaponUI();
		SwitchWeapon(baseWeapon);
	}

	public void UseWeaponDurability(int durability)
	{
		_CurrentWeaponDurability -= durability;
		OnDurabilityChanged(_CurrentWeaponDurability);
	}


	public void PlayWeaponVFX()
	{
		//GameObject vfx = Instantiate(_CurrentWeapon._WeaponAttackFX, _VfxSpawnPos.position, Quaternion.identity);
		GameObject vfx = ObjectPoolManager.GetObjectFromPool(_CurrentWeapon._WeaponAttackFX, _VfxSpawnPos.position, Quaternion.identity, ObjectPoolManager.PoolFolder.Particle);
		//Vector3 rotation = player._Actor.transform.forward;
		Quaternion rotation = player._Actor.transform.rotation;
		vfx.transform.rotation = rotation;
		//vfx.transform.Rotate(rotation);
	}

	public void SetShowWeapons(bool isShown)
	{
		if (_WeaponR != null)
			_WeaponR.SetActive(isShown);
		if (_WeaponL != null)
			_WeaponL.SetActive(isShown);
	}

	// end
}
