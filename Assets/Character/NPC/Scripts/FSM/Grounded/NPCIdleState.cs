using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : NPCGroundedState
{
    public NPCIdleState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName) : base(npc, stateMachine, animationName)
    {
    }
    /*
     * Wait for a certain amount of time
     * When that time expires, roll an int-d6
     * If entity rolls a 4+, entity picks a new area to go to
     * else time resets, roll a float-d4
     */
}
