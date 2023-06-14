using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCards : MonoBehaviour
{
    private PlayerCharacter player;

    [Tooltip("The Deck is where all the player's usable are kept.")]
    [field: SerializeField] public CardScriptableObj[] _Deck { get; private set; }
    
    [Tooltip("When a card is used, it goes to the Abyss")]
    [field: SerializeField] public CardScriptableObj[] _Abyss { get; private set; }
    
    [Tooltip("The player's hand is where the active cards are stored")]
    [field: SerializeField] public CardScriptableObj[] _Hand { get; private set; }
    
    public void Initialize(PlayerCharacter pl)
    {
        player = pl;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestUseCard()
    {
        _Hand[0].PlayCard(player);
    }
}
