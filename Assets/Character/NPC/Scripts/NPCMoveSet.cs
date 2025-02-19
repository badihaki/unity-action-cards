using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCMoveSet : MonoBehaviour
{
	[field: SerializeField]
	public List<NPCAttackActionSO> attackActions { get; private set; }

	[field: SerializeField]
	public List<NPCAttackSuperState> loadedAttacks { get; private set; }
	[field: SerializeField]
	public int attackIndex { get; private set; } = 0;

	[SerializeField]
	public NPCAttackActionSO GetCurrentAttack() => attackActions[attackIndex];
	public NPCAttackSuperState GetCurrentAttackState() => loadedAttacks[attackIndex];

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
			loadedAttacks.Add(addAction);
        }
    }

	public void SetAttackIndex(int indexNum) => attackIndex = indexNum;
	public void SetAttackIndexRandomly()
	{
		if (attackActions.Count == 1)
		{
			if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
				print("setting random atk index to 0, only 1 attack loaded");
			attackIndex = 0;
			return;
		}
		else
		{
			int newAttackIndex = Random.Range(0, attackActions.Count - 1);
			if (newAttackIndex == attackIndex) // if its the same index
			{
				List<NPCAttackSuperState> nextAttacks = new List<NPCAttackSuperState>(loadedAttacks); // copy that list
				nextAttacks.RemoveAt(attackIndex); // and remove the old attack
				// does that leave us with one attack?
				if (nextAttacks.Count == 1)
				{
					newAttackIndex = loadedAttacks.IndexOf(loadedAttacks.Find(attack => attack == nextAttacks[0]));
					if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
						print($"there's only one other attack so we're going with the next attack with index of {newAttackIndex}");
					attackIndex = newAttackIndex;
					return;
				}
				else // we should have enough to run the same logic over again, but with one less attack
				{
					newAttackIndex = Random.Range(0, nextAttacks.Count - 1);
					attackIndex = loadedAttacks.IndexOf(loadedAttacks.Find(attack => attack == nextAttacks[newAttackIndex]));
					if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
						print($"setting random atk index to {attackIndex} from range of 0 to {(attackActions.Count-1).ToString()}");
					return;
				}
			}
			print($"New attack index is {newAttackIndex} and that's what we're going with");
			attackIndex = newAttackIndex;
			return;
		}
	}
}
