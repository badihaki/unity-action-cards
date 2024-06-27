using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCMoveState : NPCState
{
    public override void EnterState()
    {
        base.EnterState();
        _NPC._NavigationController.MoveToPatrolLocation();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (Math.Round(_NPC.transform.position.x, 0) == Math.Round(_NPC._NavigationController._TargetLocation.x, 0) && Math.Round(_NPC.transform.position.z) == Math.Round(_NPC._NavigationController._TargetLocation.z))
        {
            _NPC._NavigationController.DestroyDebugObject();
            _NPC._StateMachine.ChangeState(_NPC._IdleState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!_IsExitingState)
        {
            Debug.Log("trying to get to position x " + Math.Round(_NPC._NavigationController._TargetLocation.x, 0));
        }
    }
}
