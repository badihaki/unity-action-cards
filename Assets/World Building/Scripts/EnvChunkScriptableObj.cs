using UnityEngine;

public class EnvChunkScriptableObj : ScriptableObject
{
	[field: Header("Positioning Options"), SerializeField]
	public bool isBorder;
	[field: SerializeField]
	public bool isCorner;
}
