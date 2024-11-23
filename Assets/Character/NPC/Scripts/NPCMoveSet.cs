using System.Collections.Generic;
using UnityEngine;

public class NPCMoveSet : MonoBehaviour
{
	[field:SerializeField]
	public List<NPCAttackActionSO> attackActions {  get; private set; }
	[field: SerializeField]
	public int attackIndex { get; private set; }
}
