using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoomManager : MonoBehaviour
{
    public NonPlayerCharacter npc;
    public List<NPCSheetScriptableObj> spawnableCharacters;
    [field: SerializeField] private int maxNumberOfSpawns = 4;
    [field: SerializeField] private int currentNpcCount = 0;
	private WaitForSeconds spawnNpcWait = new WaitForSeconds(8.5f);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		SpawnNewNPC();
        StartSpawnNewNpc();
	}

    private void StartSpawnNewNpc()
    {
		if (GameManagerMaster.GameMaster.GMSettings.logNPCUtilData)
			print("Starting the spawn timer");
        StartCoroutine(SpawnNPCTimer());
    }

    private IEnumerator SpawnNPCTimer()
	{
		while (currentNpcCount < maxNumberOfSpawns)
		{
			yield return spawnNpcWait;
			SpawnNewNPC();
		}
	}

	private void SpawnNewNPC()
	{
		//NonPlayerCharacter activeNpc = Instantiate(npc, transform.position, Quaternion.identity);
		GameObject pooledNpc = ObjectPoolManager.GetObjectFromPool(npc.gameObject, transform.position, Quaternion.identity, ObjectPoolManager.PoolFolder.Character);
		NonPlayerCharacter activeNpc = pooledNpc.GetComponent<NonPlayerCharacter>();
		int spawnIndex = Random.Range(0, spawnableCharacters.Count);
		if (GameManagerMaster.GameMaster.GMSettings.devMode)
			print($"spawn index is {spawnIndex}");
		activeNpc.BuildAndInitialize(spawnableCharacters[spawnIndex]);
		activeNpc._Actor.onDeath += OnNpcDeath;
		//activeNpc._Actor.onDeath -= OnNpcDeath;
		currentNpcCount++;
	}

	private void OnNpcDeath()
	{
		if (currentNpcCount == 4)
			StartSpawnNewNpc();
		currentNpcCount--;
		if(currentNpcCount <= 0)
		{
			SpawnNewNPC();
		}
	}

	// end
}
