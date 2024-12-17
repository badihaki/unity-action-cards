using System.Collections;
using UnityEngine;

public class CombatRoomManager : MonoBehaviour
{
    public NonPlayerCharacter npc;
    [field: SerializeField] private NonPlayerCharacter activeNpc;
	private WaitForSeconds spawnNpcWait = new WaitForSeconds(1.5f);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartSpawnNewNpc();
	}

    private void StartSpawnNewNpc()
    {
        StartCoroutine(SpawnNPC());
    }

    private IEnumerator SpawnNPC()
    {
        if(activeNpc != null)
            activeNpc._Actor.onDeath -= StartSpawnNewNpc;

        yield return spawnNpcWait;

		activeNpc = Instantiate(npc, transform.position, Quaternion.identity);
        activeNpc.transform.SetParent(GameManagerMaster.GameMaster.GeneralConstantVariables.CharactersFolder(), true);
        activeNpc.Initialize();
        activeNpc.name = "Fred the NPC";
        activeNpc._Actor.onDeath += StartSpawnNewNpc;
	}

	// end
}
