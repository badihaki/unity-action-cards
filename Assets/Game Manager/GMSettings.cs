using UnityEngine;

[CreateAssetMenu(fileName = "GMSettings", menuName = "Game Manager/GM Settings")]
public class GMSettings : ScriptableObject
{
	[field: Header("Developer Tools"), SerializeField]
	public bool devMode { get; private set; }

	[field: Header("Player Tools"), SerializeField]
	public bool logExraPlayerData { get; private set; }
	[field:SerializeField]
	public bool LogCardPlayerData {  get; private set; }

	[field: Header("NPC Tools"), SerializeField]
	public bool logExtraNPCData { get; private set; }
	[field: SerializeField]
	public bool logNPCNavData { get; private set; }
	[field: SerializeField]
	public bool logNPCUtilData { get; private set; }

	[field: Header("Combat"), SerializeField]
	public bool logPlayerCombat { get; private set; }
	[field:SerializeField]
	public bool logNPCCombat { get; private set; }

	//end
}
