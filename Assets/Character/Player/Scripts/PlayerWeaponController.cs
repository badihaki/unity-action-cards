using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
	private PlayerCharacter player;
	private PlayerAttack attackController;
	public delegate void ChangeWeaponDurability(int durability);
	public event ChangeWeaponDurability OnDurabilityChanged;

	[field: SerializeField] private bool infiniteDurability;

	[field: SerializeField, Header("Base Weapon")] private WeaponScriptableObj baseWeapon;
	[field: SerializeField, Header("Current Weapon")] public WeaponScriptableObj _CurrentWeapon { get; private set; }

	[field: Header("Weapon Transforms"), SerializeField] public Transform _WeaponHolderL { get; private set; }
	[field: SerializeField] public GameObject _WeaponL { get; private set; }
	[field: SerializeField] public Transform _WeaponHolderR { get; private set; }
	[field: SerializeField] public GameObject _WeaponR { get; private set; }
	public void Initialize(PlayerCharacter newPlayer)
	{
		player = newPlayer;
		attackController = player._AttackController;
		_WeaponHolderR = player._PlayerActor.RightWeapon;
		_WeaponHolderL = player._PlayerActor.LeftWeapon;
		_WeaponR = null;
		_WeaponL = null;
		SetWeapon(baseWeapon);
		_WeaponR.gameObject.SetActive(false);
	}

	private void SetWeapon(WeaponScriptableObj newWeapon)
	{
		_CurrentWeapon = newWeapon;
		LoadWeaponGameObjects(_CurrentWeapon._WeaponGameObjectL, _CurrentWeapon._WeaponGameObjectR);
		attackController.LoadMoveset(newWeapon._MoveSet);
	}

	private void LoadWeaponGameObjects(GameObject weaponL = null, GameObject weaponR = null)
	{
		if (weaponL != null)
		{
			_WeaponL = Instantiate(weaponL, _WeaponHolderL);
		}
		else print("no left-hand weapon");
		if (weaponR != null)
		{
			_WeaponR = Instantiate(weaponR, _WeaponHolderR);
		}
		else print("no right-hand weapon");
	}

	public void SwitchWeapon(WeaponScriptableObj weapon)
	{
		player._AnimationController.SetBool(_CurrentWeapon._WeaponType.ToString(), false);
		DestroyWeaponGameObjects();
		attackController.UnloadMoveSet();
		SetWeapon(weapon);
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
}
