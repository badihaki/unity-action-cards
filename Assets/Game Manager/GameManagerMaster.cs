using UnityEngine;

public class GameManagerMaster : MonoBehaviour
{
    public static GameManagerMaster GameMaster { get; private set; }
    public SaveLoadManager SaveLoadManager { get; private set; }
    public CharCustomizationDatabase CharacterCustomizationDatabase { get; private set; }
    public GMDice Dice { get; private set; }
    public ConstantVariables GeneralConstantVariables { get; private set; }
    public GameResources Resources { get; private set; }
    public static PlayerCharacter Player { get; set; }
    public GMSceneManager SceneManager { get; private set; }
    public GMCardsManager CardsManager { get; private set; }

    
    [field: SerializeField]
    public GMSettings GMSettings { get; private set; }

	// Start is called before the first frame update
	void Start()
    {
        if (GameMaster != null && GameMaster != this)
        {
            Debug.LogError("More than 1 Game Master Detected");
            Debug.LogWarning("There can be only one!!");
            Destroy(gameObject);
        }
        else
        {
            GameMaster = this;
            DontDestroyOnLoad(gameObject);
        }

        SaveLoadManager = new SaveLoadManager();
        CharacterCustomizationDatabase = GetComponent<CharCustomizationDatabase>();
        Dice = GetComponent<GMDice>();
        GeneralConstantVariables = GetComponent<ConstantVariables>();
        Resources = GetComponent<GameResources>();
        SceneManager = GetComponent<GMSceneManager>();
        CardsManager = GetComponent<GMCardsManager>();
    }

    public static void SetPlayer(PlayerCharacter newChallenger) => Player = newChallenger;

    
	public void QuitGame()
	{
		if (Application.isEditor)
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#endif
		}
		else
		{
			Application.Quit();
		}
	}
}
