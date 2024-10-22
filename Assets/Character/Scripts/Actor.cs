using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour, IKnockbackable
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
}
