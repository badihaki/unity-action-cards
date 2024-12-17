using UnityEngine;

public class ConstantVariables : MonoBehaviour
{
	[field: Header("Force values"), SerializeField, Tooltip("the force used to launch a character")]
	public float CharacterLaunchForce { get; private set; } = 25.85f;
	
	[field: SerializeField, Tooltip("X is for the pushback power (Z) and Y is for the launch force (Y)")]
	public Vector2 FarKnockBackForce { get; private set; } = new Vector2(15.3f, 23.4f);

	[field:SerializeField, Tooltip("This transform is a gameobject that holds all the characters in the game, and if there is no folder it will make it.")]
	public Transform CharactersFolder()
	{
		Transform folder = GameObject.Find("Characters").transform;
		if (folder != null)
			return folder;
		else
		{
			Transform newFolder = new GameObject().transform;
			newFolder.name = "Characters";
			return newFolder;
		}
	}
}
