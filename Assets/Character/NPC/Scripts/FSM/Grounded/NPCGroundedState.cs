using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGroundedState : NPCState
{
    public NPCGroundedState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName) : base(npc, stateMachine, animationName)
    {
    }
}
