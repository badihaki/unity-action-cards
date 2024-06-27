using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NPCIdleState : NPCState
{
    protected float waitTime;

    public override void EnterState()
    {
        base.EnterState();

        CreateNewWait(2.5f, 7.0f);
    }

    private void CreateNewWait(float min, float max)
    {
        Debug.Log(_NPC.name + " is creating a new wait time: " + min + ", " + max);
        waitTime = GameManagerMaster.GameMaster.Dice.RollRandomDice(min, max);
        Debug.Log(_NPC.name + "'s new wait time: " + waitTime);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        waitTime -= Time.deltaTime;
        if(waitTime <= 0)
        {
            waitTime = 0;
            FindPlaceToGo();
        }
    }

    private void RollForWait()
    {
        int roll = GameManagerMaster.GameMaster.Dice.RollD6();
        Debug.Log(_NPC.name + " is Rolling to wait. Need 5+ to ignore. Roll?: " + roll);
        if (roll >= 5)
        {
            FindPlaceToGo();
            return;
        }
        Debug.Log(_NPC.name + " is making a new wait");
        float minWait = GameManagerMaster.GameMaster.Dice.RollRandomDice(1.3f, 2.0f);
        float maxWait = GameManagerMaster.GameMaster.Dice.RollRandomDice(3.5f, 5.0f);
        CreateNewWait(minWait, maxWait);
    }

    private void FindPlaceToGo()
    {
        Debug.Log(_NPC.name + " is Finding a place to move to");
        if (_NPC._NavigationController.TryFindNewPatrol())
        {
            _NPC._StateMachine.ChangeState(_NPC._MoveState);
        }
        else
        {
            RollForWait();
        }
    }
}
