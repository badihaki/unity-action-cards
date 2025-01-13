using UnityEngine;

[CreateAssetMenu(fileName = "GMSettings", menuName = "Game Manager/GM Settings")]
public class GMSettings : ScriptableObject
{
	[field: Header("Developer Tools"), SerializeField]
	public bool devMode { get; private set; }
	[field: SerializeField]
	public bool logExtraNPCData { get; private set; }
	[field: SerializeField]
	public bool logExraPlayerData { get; private set; }
	[field:SerializeField]
	public bool LogCardPlayerData {  get; private set; }
}
