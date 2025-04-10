using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField]
    private Transform playerStartPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		CharacterController charControl = GameManagerMaster.Player._PlayerActor.GetComponent<CharacterController>();
		charControl.enabled = false;
		GameManagerMaster.Player._PlayerActor.transform.position = playerStartPosition.position;
		charControl.enabled = true;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
