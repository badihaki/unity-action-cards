using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHurtSuperState : NPCState
{
    public override void AnimationEndTrigger()
    {
        base.AnimationEndTrigger();

        if (!_NPC._AttackController._IsAggressive)
        {
            _StateMachine.ChangeState(_NPC._IdleState);
        }
        else
        {
            _StateMachine.ChangeState(_NPC._IdleAggressiveState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        _NPC.EndHurtAnimation();
    }
}
