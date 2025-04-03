using UnityEngine;

[CreateAssetMenu(fileName ="prop_",menuName = "WorldGen/Props/New Prop")]
public class PropScriptableObj : ScriptableObject
{
	public string propName;
	public int health;
	public GameObject propGameObject;
}
