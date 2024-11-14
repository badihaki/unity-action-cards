using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Actor : MonoBehaviour, IKnockbackable, IDamageable
{
    [field: SerializeField, Header("Character Actor Basics")]
    private Character _Character;
    [field: SerializeField]
    public Animator animationController { get; protected set; }

    [field: SerializeField, Header("Components")]
    public CheckForGround _CheckGrounded { get; private set; }

    [field:SerializeField, Header("FXs")] private GameObject bloodFX;


	public bool _IsInvuln { get; private set; }

    public virtual void Initialize(Character character)
    {
        _Character = character;
        animationController = GetComponent<Animator>();
        // animationController.ApplyBuiltinRootMotion();
        animationController.applyRootMotion = true;
        _CheckGrounded = GetComponent<CheckForGround>();
        _CheckGrounded.Initialize();
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

	public virtual void ApplyKnockback(Transform forceSource, float knockforce, float launchForce)
	{
        //print($"applying knockback to actor {name} controlled by {_Character}");
	}

	public virtual void Damage(int damage, Transform damageSource, float knockForce, float launchForce, Character damageSourceController = null)
	{
		if (damageSource != _Character.transform && !_IsInvuln)
		{
			_Character._Health.TakeDamage(damage);
			_Character._Actor.transform.LookAt(damageSource);

			Quaternion rotation = _Character._Actor.transform.rotation;
			rotation.x = 0;
			rotation.z = 0;

			_Character._Actor.transform.rotation = rotation;

			_Character.CalculateHitResponse(knockForce, launchForce, damage);
			ApplyKnockback(damageSource, knockForce, launchForce);
			//DetermineWhoWhurtMe(damageSource);

			if (bloodFX != null)
			{
				GameObject blood = Instantiate(bloodFX, transform.position, rotation);
				blood.transform.rotation = transform.rotation;
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
        //
    }
}
