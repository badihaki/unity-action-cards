using System.Collections.Generic;
using UnityEngine;

public class NPCMoveSet : MonoBehaviour
{
	[field: SerializeField]
	public List<NPCAttackActionSO> attackActions { get; private set; }

	[field: SerializeField]
	public List<NPCAttackSuperState> loadedAttackActions { get; private set; }
	[field: SerializeField]
	public int attackIndex { get; private set; } = 0;

	[SerializeField]
	public NPCAttackActionSO GetCurrentAttack() => attackActions[attackIndex];
	public NPCAttackSuperState GetCurrentAttackState() => loadedAttackActions[attackIndex];

	public void Initialize(NonPlayerCharacter npc)
	{
		NPCSheetScriptableObj characterSheet = npc._CharacterSheet as NPCSheetScriptableObj;

		foreach (NPCAttackActionSO action in characterSheet.StateLibrary._Attacks)
        {
			//NPCAttackSuperState addAction = ScriptableObject.CreateInstance<NPCAttackSuperState>();
			NPCAttackSuperState addAction = Instantiate(action.attackState);
			addAction.InitState(npc, npc._StateMachine, action.animationName);
			addAction.name = action.name;
			attackActions.Add(action);
			loadedAttackActions.Add(addAction);
        }
    }

	public void SetAttackIndex(int indexNum) => attackIndex = indexNum;
	public void SetAttackIndexRandomly()
	{
		if(attackActions.Count == 1)
		{
			print("setting random atk index to 0, only 1 attack loaded");
			attackIndex = 0;
			return;
		}
		attackIndex = Random.Range(0, attackActions.Count - 1);
		print($"setting random atk index {attackIndex}");
	}
}
