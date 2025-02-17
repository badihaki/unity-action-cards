using UnityEngine;

public class ConstantVariables : MonoBehaviour
{
	[field: Header("Force values"), SerializeField, Tooltip("the force used to launch a character")]
	public float CharacterLaunchForce { get; private set; } = 25.85f;
	
	[field: SerializeField, Tooltip("X is for the pushback power (Z) and Y is for the launch force (Y)")]
	public Vector2 FarKnockBackForce { get; private set; } = new Vector2(15.3f, 23.4f);

	[field: SerializeField, Tooltip("This transform is a gameobject that holds all the characters in the game, and if there is no folder it will make it.")]
	private Transform CharactersFolder;
	public Transform GetCharactersFolder()
	{
		if (CharactersFolder != null)
			return CharactersFolder;
		else
			return CreateNewFolder("Characters");
	}

	[field: SerializeField, Tooltip("This transform is a gameobject that holds all the projectiles in the game, and if there is no folder it will make it.")]
	private Transform ProjectilesFolder;
	public Transform GetProjectilesFolder()
	{
		if (ProjectilesFolder != null)
			return ProjectilesFolder;
		else
			return CreateNewFolder("Projectiles");
	}

	[field: SerializeField, Tooltip("This transform is a gameobject that holds all the particles in the game, and if there is no folder it will make it.")]
	private Transform ParticlesFolder;
	public Transform GetParticlesFolder()
	{
		if (ParticlesFolder != null)
			return ParticlesFolder;
		else
			return CreateNewFolder("Particles");
	}

	private Transform CreateNewFolder(string folderName)
	{
		Transform newFolder = new GameObject().transform;
		newFolder.name = folderName;
		return newFolder;
	}
}
