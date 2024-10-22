using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Actor : MonoBehaviour, IKnockbackable, IDamageable
{
    private Character _Character;
    [field: SerializeField] public Animator animationController { get; protected set; }

    [field: SerializeField, Header("Animator Movement")]
    public Vector3 animatorMovementVector { get; protected set; } = new Vector3();
    [field: SerializeField] protected bool controlByRootMotion = false;

    [field: SerializeField, Header("Components")]
    public CheckForGround CheckGrounded { get; private set; }

    public virtual void Initialize(Character character)
    {
        _Character = character;
        animationController = GetComponent<Animator>();
        animationController.ApplyBuiltinRootMotion();
        animationController.applyRootMotion = true;
        CheckGrounded = GetComponent<CheckForGround>();
        CheckGrounded.Initialize();
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
        print($"applying knockback to actor {name} controlled by {_Character}");
	}

	public void Damage(int damage, Transform damageSource, float knockForce, float launchForce)
	{
		if (damageSource != _Character.transform)
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
		}
	}

	public Transform GetControllingEntity()
	{
		return _Character.transform;
	}
}
