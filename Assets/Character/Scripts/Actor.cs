using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private Character Character;
    [field: SerializeField] public Animator animationController { get; protected set; }

    [field: SerializeField, Header("Animator Movement")]
    public Vector3 animatorMovementVector { get; protected set; } = new Vector3();
    [field: SerializeField] protected bool controlByRootMotion = false;

    public virtual void Initialize(Character character)
    {
        Character = character;
        animationController = GetComponent<Animator>();
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
}
