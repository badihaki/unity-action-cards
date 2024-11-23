using UnityEngine;

[CreateAssetMenu(fileName = "AttackAct", menuName = "Characters/NPC/Attacks/New Action")]
public class NPCAttackActionSO : ScriptableObject
{
	[field: SerializeField]
	public int damage { get; private set; } = 1;

	[field: SerializeField]
	public NPCAttackSuperState attackSuperState { get; private set; }
}
