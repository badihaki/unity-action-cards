using UnityEngine;
using UnityEngine.SceneManagement;

public class GMSceneManager : MonoBehaviour
{
	[SerializeField]
	private int currentSceneIndex;
	public void ChangeScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}
}
