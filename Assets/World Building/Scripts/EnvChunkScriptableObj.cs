#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "envChunk_", menuName = "WorldGen/Create new Environment Chunk")]
public class EnvChunkScriptableObj : ScriptableObject
{
	[field: SerializeField]
	public bool isBorder { get; private set; }
	[field: SerializeField]
	public bool isCorner { get; private set; }
	[field: SerializeField]
	public bool north { get; private set; }
	[field: SerializeField]
	public bool south { get; private set; }
	[field: SerializeField]
	public bool east { get; private set; }
	[field: SerializeField]
	public bool west { get; private set; }

	[field:SerializeField]
	public EnvChunk chunkGameObj { get; private set; }
	[field: SerializeField]
	public List<GameObject> buildingTemplates { get; private set; }
	[field: SerializeField]
	public List<PropScriptableObj> floraPropTemplates { get; private set; }
	[field: SerializeField]
	public List<NonPlayerCharacter> regularNPCs { get; private set; }
	[field: SerializeField]
	public List<NonPlayerCharacter> specialNPCs { get; private set; }
	[field: SerializeField]
	public List<NonPlayerCharacter> enemies { get; private set; }
	[field: SerializeField]
	public List<NonPlayerCharacter> specialEnemies { get; private set; }
}
