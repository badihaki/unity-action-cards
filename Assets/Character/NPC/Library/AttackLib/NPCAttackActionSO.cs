using UnityEngine;

[CreateAssetMenu(fileName = "AttackAct", menuName = "Characters/NPC/Combat/New Attack Action")]
public class NPCAttackActionSO : ScriptableObject
{
	[field: SerializeField,Header("Basics")]
	public string animationName { get; private set; }
	[field: SerializeField]
	public int damage { get; private set; } = 1;
	[field: SerializeField, Tooltip("This is the actual state logic for this attack")]
	public NPCAttackSuperState attackState { get; private set; }
	[field: SerializeField, Tooltip("When requesting an attack, use this to determine which attacks can be chosen. EG general attacks have lower priority, stronger attacks have more priority")]
	public int attackPriority { get; private set; } = 0;
	//
	[field: SerializeField, Header("Distance Variables")]
	public float mininumDistance { get; private set; } = 0.56f;
	[field: SerializeField]
	public float desiredDistance { get; private set; } = 1.23f;
	[field: SerializeField]
	public float maxinumDistance { get; private set; } = 2.0f;

	//
	[field: SerializeField, Header("Wait Time")]
	public float waitTime { get; private set; } = 1.5f;
}
