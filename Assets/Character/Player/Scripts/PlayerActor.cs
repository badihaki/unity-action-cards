using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerActor : Actor
{
    private PlayerCharacter PCActor;
    public enum bodyTypes
    {
        male,
        female
    }
    public bodyTypes bodyType { get; private set; }

    [Header("Bones and Transforms")]
    [field: SerializeField] private Transform rootBone;
    [field: SerializeField] public Transform RightWeapon { get; private set; }
    [field: SerializeField] public Transform LeftWeapon { get; private set; }


    public void InitializePlayerActor(PlayerCharacter character)
    {
        base.Initialize(character);
        PCActor = character;
    }

    private void OnAnimatorMove()
    {
        if (animationController && controlByRootMotion)
        {
            animatorMovementVector = animationController.deltaPosition.normalized;
            PCActor.transform.position += animationController.deltaPosition;
            animationController.rootRotation = PCActor.transform.rotation;
        }
    }

    public void SetSyncParentMotion(bool value) => controlByRootMotion = value;

    public void SendPositionDataToParent(PlayerCharacter parent)
    {
        OnAnimatorMove();
        // parent.transform.position += animationController.deltaPosition.normalized;
        parent.transform.position += animationController.deltaPosition;
    }

    public override void StateAnimationFinished() => PCActor.StateAnimationFinished();
    public override void AnimationTrigger() => PCActor.StateTrigger();
    public override void AnimationVFXTrigger() => PCActor.StateVFXTrigger();
    public override void AnimationSFXTrigger() => PCActor.StateSFXTrigger();
}
