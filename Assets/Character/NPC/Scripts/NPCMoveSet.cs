using System.Collections.Generic;
using UnityEngine;

public class NPCMoveSet : MonoBehaviour
{
	[field:SerializeField]
	public List<NPCAttackActionSO> attackActions {  get; private set; }
	[field: SerializeField]
	public List<NPCAttackSuperState> loadedAttackActions { get; private set; }
	[field: SerializeField]
	public int attackIndex { get; private set; } = 0;

	public NPCAttackActionSO GetCurrentAttack() => attackActions[attackIndex];
	public NPCAttackSuperState GetCurrentAttackState() => loadedAttackActions[attackIndex];

	public void Initialize(NonPlayerCharacter npc)
	{
        foreach (NPCAttackActionSO action in attackActions)
        {
			NPCAttackSuperState addAction = ScriptableObject.CreateInstance<NPCAttackSuperState>();
			addAction.InitState(npc, npc._StateMachine, action.animationName);
			addAction.name = action.name;
			loadedAttackActions.Add(addAction);
        }
    }

	public void SetAttackIndex(int indexNum) => attackIndex = indexNum;
	public void SetAttackIndexRandomly()
	{
		if(attackActions.Count <= 1)
		{
			print("setting random atk index to 0, only 1 attack loaded");
			attackIndex = 0;
			return;
		}
		attackIndex = Random.Range(0, attackActions.Count - 1);
		print($"setting random atk index {attackIndex}");
	}
}
