using System;
using UnityEngine;

public class Actor : MonoBehaviour, IDamageable
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

    public virtual void Damage(int damage, Transform damageSource, bool knockedBack = false, bool launched = false, Character damageSourceController = null)
	{
		if (damageSource != _Character.transform && !_IsInvuln)
		{
			_Character._Health.TakeDamage(damage);
			_Character._Actor.transform.LookAt(damageSource);

			Quaternion rotation = _Character._Actor.transform.rotation;
			rotation.x = 0;
			rotation.z = 0;

			_Character._Actor.transform.rotation = rotation;

			_Character.CalculateHitResponse(knockedBack, launched, damage); // need to rewrite thiss

			if (bloodFX != null)
			{
				GameObject blood = Instantiate(bloodFX, hitFxOrigin.position, rotation);
				blood.transform.rotation = hitFxOrigin.rotation;
			}
		}
	}

    public void SetInvulnerability(int value)
    {
        bool res = Convert.ToBoolean(value);
        //print($"value is {res}");
        if(_IsInvuln != res) _IsInvuln = res;
    }

	public Transform GetControllingEntity()
	{
		return _Character.transform;
	}

	public Transform GetDamagedEntity()
	{
        return transform;
	}

    public virtual void Die()
    {
        if (onDeath != null)
            onDeath();
    }
}
