using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
	private Character _Character;

	[field: Header("Attack Stats"), SerializeField]
	public int _Damage { get; protected set; }
	[field: SerializeField] public float _KnockbackForce { get; protected set; }
	[field: SerializeField] public float _LaunchForce { get; protected set; }

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public virtual void Initialize(Character character)
	{
		_Character = character;
	}

	public virtual void SetAttackParameters(float knockbackForce, float launchForce, int damageModifier = 0)
	{
		_KnockbackForce = knockbackForce;
		_LaunchForce = launchForce;
	}

	public void ResetAttackParameters()
	{
		_Damage = 0;
		_KnockbackForce = 0.0f;
		_LaunchForce = 0.0f;
	}
}
