using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
	private Character _Character;

	[field: Header("Attack Stats"), SerializeField]
	public int _Damage { get; protected set; }
	[field: SerializeField] public bool _KnockedBack { get; protected set; } = false;
	[field: SerializeField] public bool _Launched { get; protected set; } = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public virtual void Initialize(Character character)
	{
		_Character = character;
	}

	public virtual void SetAttackParameters(bool knockback, bool launch, int damageModifier = 0)
	{
		_KnockedBack = knockback;
		_Launched = launch;
	}

	public void ResetAttackParameters()
	{
		_Damage = 0;
		_KnockedBack = false;
		_Launched = false;
	}
}
