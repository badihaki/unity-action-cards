using UnityEngine;

public class GameResources : MonoBehaviour
{
	[SerializeField]
	private static GameResources _GameResources;
	public static GameResources _Resources
	{
		get
		{
			return _GameResources;
		}
	}


	[Header("Templates")]
	public GameObject npcTemplate;
	public CorruptionHeart corruptionHeart;

	[Header("Effects")]
	public GameObject fireElementEffect;


	// end
}
