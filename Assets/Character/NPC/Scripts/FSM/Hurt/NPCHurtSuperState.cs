using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHurtSuperState : NPCState
{
    public override void AnimationEndTrigger()
    {
        base.AnimationEndTrigger();

        _StateMachine.ChangeState(_NPC._IdleState);
    }

    public override void ExitState()
    {
        base.ExitState();

        _NPC.EndHurtAnimation();
    }
}
