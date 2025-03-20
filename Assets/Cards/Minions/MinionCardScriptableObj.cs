using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "Create Card/New Minion")]
public class MinionCardScriptableObj : CardScriptableObj
{
    [Header("Minion Specific Below")]
    [Tooltip("Minions will need a character sheet to initialize")]
    public NPCSheetScriptableObj _MinionCharacterSheet;

    protected override void UseCardAbility(Character controllingCharacter)
    {
        if(controllingCharacter is PlayerCharacter player)
        {
            if (!player._MinionController.TrySummonMinion(_MinionCharacterSheet))
            {
                Debug.Log($"{player.name} cant summon");
                player._PlayerCards.RefundCardPlay();
            }
        }
	}

	// end of the class
}
