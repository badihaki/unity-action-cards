using System;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [field: SerializeField, Header("Character Actor Basics")]
    private Character _Character;
    [field: SerializeField]
    public Animator animationController { get; protected set; }

    [field: SerializeField, Header("Components")]
    public CheckForGround _CheckGrounded { get; private set; }

    [field:SerializeField, Header("FXs")]
    private GameObject bloodFX;

    [field: SerializeField, Header("Transforms")]
    public Transform hitFxOrigin { get; protected set; }

    // invulnerability
	public bool _IsInvuln { get; private set; }


	// events
	public delegate void DeathSideEffects();
	public event DeathSideEffects onDeath;

	public virtual void Initialize(Character character)
    {
        _Character = character;
        animationController = GetComponent<Animator>();
        // animationController.ApplyBuiltinRootMotion();
        animationController.applyRootMotion = true;
        _CheckGrounded = GetComponent<CheckForGround>();
        _CheckGrounded.Initialize();

		FindTransforms();
    }

	private void FindTransforms()
	{
        hitFxOrigin = transform.Find("HitOrigin");
	}

	// TODO:: Add this to Character ~OR~ Make the methods empty so Derived classes can override
	public virtual void StateAnimationFinished()
    {
        //
    }
    public virtual void AnimationTrigger()
    {
        //
    }
    public virtual void AnimationVFXTrigger()
    {
        //
    }
    public virtual void AnimationSFXTrigger()
    {
        //
    }

    //public virtual void Damage(int damage, Transform damageSource, bool knockedBack = false, bool launched = false, Character damageSourceController = null)
    public virtual void TakeDamage(Damage dmgObj)
    {
        if (dmgObj.damageSource != _Character.transform && !_IsInvuln)
		{
			UseDamageObj(dmgObj);
		}
	}

	private void UseDamageObj(Damage dmgObj)
	{
		_Character._Health.TakeDamage(dmgObj.damageAmount);
		_Character._Actor.transform.LookAt(dmgObj.damageSource);

		// calculate rotation
		Quaternion rotation = CalculateRotationWhenDmg();

        bool launched = false;
        if (dmgObj.intendedResponse == responsesToDamage.launch || dmgObj.intendedResponse == responsesToDamage.knockBack)
            launched = true;

        _Character.PushBackCharacter(dmgObj.damageSource.position, dmgObj.damageForce, launched);
		_Character.RespondToHit(dmgObj.intendedResponse);

		BleedWhenDmg(rotation);
	}

	private void BleedWhenDmg(Quaternion rotation)
	{
		if (bloodFX != null)
		{
			//GameObject blood = Instantiate(bloodFX, hitFxOrigin.position, rotation);
			GameObject blood = ObjectPoolManager.GetObjectFromPool(bloodFX, hitFxOrigin.position, rotation, ObjectPoolManager.PoolFolder.Particle);
			blood.transform.rotation = hitFxOrigin.rotation;
		}
	}

	private Quaternion CalculateRotationWhenDmg()
	{
		Quaternion rotation = _Character._Actor.transform.rotation;
		rotation.x = 0;
		rotation.z = 0;
		_Character._Actor.transform.rotation = rotation;
		return rotation;
	}

	public void SetInvulnerability(int value)
    {
        bool res = Convert.ToBoolean(value);
        //print($"value is {res}");
        if(_IsInvuln != res) _IsInvuln = res;
    }

    public virtual void Die()
    {
        if (onDeath != null)
            onDeath();
    }
}
