using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleAggressiveState : NPCState
{
    private float waitTime;
    private float distanceFromPlayer;
    public override void EnterState()
    {
        base.EnterState();

        _NPC._NavigationController.StopNavigation();
        waitTime = CreateNewWait(1.2f, 3.2f);
        Debug.Log($"aggression wait time = {waitTime}");
        Debug.Log($"{_NPC._NavigationController.IsNavStopped()}");
    }

    public override void LogicUpdate()
    {
        distanceFromPlayer = Vector3.Distance(_NPC.transform.position, _NPC._NavigationController._Target.position);

        base.LogicUpdate();
        
        if (waitTime > 0) RunWaitTimer();
        
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
        if (distanceFromPlayer > _NPC._NavigationController._MaxDistance)
        {
            Debug.Log($"entity {_NPC.name} is moving towards target. Distance from target exceeded {distanceFromPlayer}");
            _StateMachine.ChangeState(_NPC._MoveState);
        }
    }

    private void RunWaitTimer()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0) Debug.Log($"done with wait");
    }

    private float CreateNewWait(float min, float max)
    {
        return GameManagerMaster.GameMaster.Dice.RollRandomDice(min, max);
    }
}
