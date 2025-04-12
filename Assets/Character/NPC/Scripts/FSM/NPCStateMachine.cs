using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
	[field: SerializeField]
	public NPCStateLibrary _StateLibrary { get; private set; }
	[field: SerializeField]
    public NPCState _CurrentState { get; private set; }
	[field: SerializeField]
	public string _StateAnimationName { get; private set; }

	private bool _Ready = false;
    

    public void InitializeStateMachine(NonPlayerCharacter npc)
    {
		NPCSheetScriptableObj characterSheet = npc._CharacterSheet as NPCSheetScriptableObj;
		//SetUpStateMachine(npc);
		_StateLibrary = ScriptableObject.CreateInstance<NPCStateLibrary>();
		_StateLibrary.InitializeAllStates(npc);

		// set current state
        _CurrentState = _StateLibrary._IdleState;
		_StateAnimationName = _CurrentState._StateAnimationName;
        _CurrentState.EnterState();
        _Ready = true;
    }

	private void OnDisable()
	{
		_Ready = false;
        _CurrentState = null;
	}

	public void GoToHurtState(responsesToDamage damageResponse) => _CurrentState.GetHurt(damageResponse);

	public void ChangeState(NPCState state)
    {
        _CurrentState.ExitState();
        _CurrentState = state;
		_StateAnimationName = _CurrentState._StateAnimationName;
		_CurrentState.EnterState();
    }

    public void Update()
    {
        if (_Ready && GameManagerMaster.GameMaster)
        {
            _CurrentState.LogicUpdate();
        }
    }

    public void FixedUpdate()
    {
        if (_Ready && GameManagerMaster.GameMaster)
        {
            _CurrentState.PhysicsUpdate();
        }
    }

	//public void LateUpdate()
	//{
	//	if (_Ready && GameManagerMaster.GameMaster)
	//	{
	//		//_CurrentState.();
	//	}
	//}

	public void LogFromState(string input)
    {
        print($">>> : {input} : <<< NPC :: {_CurrentState.ToString()}");
    }
}
