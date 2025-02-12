using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
	private Character _Character;

	[field: Header("Attack Stats"), SerializeField]
	public int _Damage { get; protected set; }
	public float _Force { get; protected set; }
	[field: SerializeField, Header("Intended Attack Response")]
	public responsesToDamage _IntendedResponseToDamageBeingDealt { get; protected set; }
	//[field: SerializeField] public bool _KnockedBack { get; protected set; } = false;
	//[field: SerializeField] public bool _Launched { get; protected set; } = false;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public virtual void Initialize(Character character)
	{
		_Character = character;
        _IntendedResponseToDamageBeingDealt = responsesToDamage.hit;
	}

	// TODO:::: wanna add force to this later
	public virtual void SetAttackParameters(responsesToDamage intendedDmgResponse = responsesToDamage.hit, int damageModifier = 0, float force = 1.0f)
	{
		// was -->> bool knockback, bool launched, int damageModifier(this was additional damage on top of base weapon dmg)
		_Damage = damageModifier;
		_Force = force;
		_IntendedResponseToDamageBeingDealt = intendedDmgResponse;
	}

	public virtual void ResetAttackParameters()
	{
		_Damage = 0;
		//_KnockedBack = false;
		//_Launched = false;
		_IntendedResponseToDamageBeingDealt = responsesToDamage.hit;
	}

	public virtual void PlayHitSpark(Vector3 hitPos) { }
}
