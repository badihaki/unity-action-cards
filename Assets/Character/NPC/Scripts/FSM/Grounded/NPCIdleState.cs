using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NPCIdleState : NPCGroundedState
{
    protected float waitTime;
    protected bool canMove;

    public override void EnterState()
    {
        base.EnterState();

        CreateNewWait(0.3f, 2.0f);
        canMove = false;
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

        if(!canMove)
        {
            waitTime -= Time.deltaTime;
            Debug.Log("wait time: " + waitTime);
            if(waitTime <= 0)
            {
                waitTime = 0;
                FindPlaceToGo();
            }
        }
        else
        {
            if (Math.Round(_NPC.transform.position.x, 0) == Math.Round(_NPC._NavigationController._TargetLocation.x, 0) && Math.Round(_NPC.transform.position.z) == Math.Round(_NPC._NavigationController._TargetLocation.z))
            {
                canMove = false;
            }
        }
    }

    private void RollForWait()
    {
        int roll = GameManagerMaster.GameMaster.Dice.RollD6();
        Debug.Log(_NPC.name + " is Rolling to wait. Need 5+ to ignore. Roll?: " + roll);
        if (roll >= 4)
        {
            FindPlaceToGo();
            return;
        }
        Debug.Log(_NPC.name + " is making a new wait");
        float minWait = GameManagerMaster.GameMaster.Dice.RollRandomDice(0.3f, 1.2f);
        float maxWait = GameManagerMaster.GameMaster.Dice.RollRandomDice(2.0f, 5.0f);
        CreateNewWait(minWait, maxWait);
    }

    private void FindPlaceToGo()
    {
        Debug.Log(_NPC.name + " is Finding a place to move to");
        if (_NPC._NavigationController.TryFindNewPatrol())
        {
            canMove = true;
            _NPC._NavigationController.MoveToPatrolLocation();
        }
        else
        {
            RollForWait();
        }
    }
}
