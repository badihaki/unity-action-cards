using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "Create Card/New Minion")]
public class MinionCardScriptableObj : CardScriptableObj
{
    [Header("Minion Specific Below")]
    [Tooltip("Minions will need a character sheet to initialize")]
    public CharacterSheet _MinionCharacterSheet;
    [Tooltip("The actor is the in-game representative of the minion. What the player sees")]
    public GameObject _MinionActor;

    public override void PlayCard(Character controllingCharacter)
    {
        base.PlayCard(controllingCharacter);

        Vector3 spawnPosition = new Vector3(controllingCharacter.transform.position.x + Random.Range(1.5f, 2.3f), controllingCharacter.transform.position.y, controllingCharacter.transform.position.z + Random.Range(1.5f, 3.5f));

        if(Physics.Raycast(spawnPosition, Vector3.forward, out RaycastHit hitinfo, 5.0f))
        {
            Debug.Log("Can't spawn here");
            return;
        }

        Instantiate(_MinionActor, spawnPosition, Quaternion.identity);
    }
}
