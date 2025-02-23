using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "Create Card/New Minion")]
public class MinionCardScriptableObj : CardScriptableObj
{
    [Header("Minion Specific Below")]
    [Tooltip("Minions will need a character sheet to initialize")]
    public NPCSheetScriptableObj _MinionCharacterSheet;
    [Tooltip("The actor is the in-game representative of the minion. What the player sees")]
    public GameObject _MinionActor;

    protected override void UseCardAbility(Character controllingCharacter)
    {
        if (controllingCharacter._Aether._CurrentAether < _CardCost)
        {
            Debug.Log("Not enough aether");
            return;
        }

        if(controllingCharacter is PlayerCharacter player)
        {
            player._MinionController.TrySummonMinion(_MinionCharacterSheet);
        }
	}

	// end of the class
}
