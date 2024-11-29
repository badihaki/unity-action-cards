using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHurtSuperState : NPCState
{
	public override void AnimationEndTrigger()
    {
        base.AnimationEndTrigger();

        if (!_NPC._NPCActor._AggressionManager.isAggressive)
        {
            _StateMachine.ChangeState(_StateMachine._IdleState);
        }
        else
        {
            _StateMachine.ChangeState(_StateMachine._IdleAggressiveState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        _NPC._MoveController.SetExternalForces(_NPC.transform, 0.0f, 0.0f);
    }
}
