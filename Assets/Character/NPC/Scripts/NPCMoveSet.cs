using System.Collections.Generic;
using UnityEngine;

public class NPCMoveSet : MonoBehaviour
{
	[field:SerializeField]
	public List<NPCAttackActionSO> attackActions {  get; private set; }
	[field: SerializeField]
	public int attackIndex { get; private set; } = 0;

	public NPCAttackSuperState GetCurrentAttack() => attackActions[attackIndex].attackSuperState;

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
