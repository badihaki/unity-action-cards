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
        rootBone = transform.Find("rig").Find("root");
        Transform chestBone = rootBone.Find("DEF-spine").Find("DEF-spine.001").Find("DEF-spine.002").Find("DEF-chest").transform;
        RightWeapon = chestBone.Find("DEF-shoulder.R").Find("DEF-upper_arm.R").Find("DEF-upper_arm.R.001").Find("DEF-forearm.R").Find("DEF-forearm.R.001").Find("DEF-hand.R").Find("DEF-weapon.R").transform;
        LeftWeapon = chestBone.Find("DEF-shoulder.L").Find("DEF-upper_arm.L").Find("DEF-upper_arm.L.001").Find("DEF-forearm.L").Find("DEF-forearm.L.001").Find("DEF-hand.L").Find("DEF-weapon.L").transform;
    }

    public override void StateAnimationFinished() => PCActor.StateAnimationFinished();
    public override void AnimationTrigger() => PCActor.StateTrigger();
    public override void AnimationVFXTrigger() => PCActor.StateVFXTrigger();
    public override void AnimationSFXTrigger() => PCActor.StateSFXTrigger();
}
