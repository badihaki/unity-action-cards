using UnityEngine;

public class PlayerInteractionCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacter player;
    
    void Awake()
    {
        if (player == null)
            player = GetComponentInParent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInteraction()
    {
        print("showing interactions");
    }

    public void CloseInteractionMenu()
    {
        player._StateMachine.ChangeState(player._StateMachine._IdleState);
        player._CameraController.LockCursorKBM();
        gameObject.SetActive(false);
    }
}
