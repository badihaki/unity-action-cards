using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
	private Character _Character;

	[field: Header("Attack Stats"), SerializeField]
	public int _DamageModifier { get; protected set; }
	public float _Force { get; protected set; }
	//[field: SerializeField] public bool _KnockedBack { get; protected set; } = false;
	//[field: SerializeField] public bool _Launched { get; protected set; } = false;

	[field: SerializeField, Header("Intended Attack Response")]
	public responsesToDamage _IntendedResponseToDamageBeingDealt { get; protected set; }

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public virtual void Initialize(Character character)
	{
		_Character = character;
        _IntendedResponseToDamageBeingDealt = responsesToDamage.hit;
	}

	// TODO:::: wanna add force to this later
	public virtual void SetAttackParameters(float damageForce, int damageModifier = 0)
	{
		//_KnockedBack = knockback;
		//_Launched = launch;
		_Force = damageForce;
		_DamageModifier = damageModifier;
	}

	public void SetIntendedDmgResponse(responsesToDamage _responsesToDamage) => _IntendedResponseToDamageBeingDealt = _responsesToDamage;

	public virtual void ResetAttackParameters()
	{
		_DamageModifier = 0;
		//_KnockedBack = false;
		//_Launched = false;
		_IntendedResponseToDamageBeingDealt = responsesToDamage.hit;
	}

	public virtual void PlayHitSpark(Vector3 hitPos) { }
}
