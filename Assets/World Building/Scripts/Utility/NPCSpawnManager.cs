using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnManager : MonoBehaviour
{
	[field: SerializeField] private bool isSpawning;
	 public bool SetIsSpawning(bool value)
	{
		isSpawning = value;
		return isSpawning;
	}
	[field: SerializeField] private NonPlayerCharacter npc;
	[field: SerializeField] private List<NPCSheetScriptableObj> spawnableCharacters;
    [field: SerializeField] private int maxNumberOfSpawns = 4;
    [field: SerializeField] private int currentNpcCount = 0;
	//private WaitForSeconds spawnNpcWait = new WaitForSeconds(8.5f);
	[field: SerializeField] private float spawnTimer = 0.0f;
	[field: SerializeField] private float spawnTimerThreshold = 12.35f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		SpawnNewNPC();
		SetIsSpawning(true);
	}

	private void FixedUpdate()
	{
		RunSpawnTimer();
	}

	private void RunSpawnTimer()
	{
		if (isSpawning && currentNpcCount < maxNumberOfSpawns)
		{
			spawnTimer += Time.deltaTime;
			if( spawnTimer >= spawnTimerThreshold )
			{
				SpawnNewNPC();
				spawnTimer = 0.0f;
			}
		}
	}

	private void SpawnNewNPC()
	{
		//NonPlayerCharacter activeNpc = Instantiate(npc, transform.position, Quaternion.identity);
		int spawnIndex = Random.Range(0, spawnableCharacters.Count);
		GameObject pooledNpc = ObjectPoolManager.GetObjectFromPool(npc.gameObject, transform.position, Quaternion.identity, ObjectPoolManager.PoolFolder.Character, spawnableCharacters[spawnIndex]._CharacterName);
		NonPlayerCharacter activeNpc = pooledNpc.GetComponent<NonPlayerCharacter>();
		if (GameManagerMaster.GameMaster.GMSettings.devMode)
			print($"spawn index is {spawnIndex}");
		activeNpc.BuildAndInitialize(spawnableCharacters[spawnIndex]);
		activeNpc._Actor.onDeath += OnNpcDeath;
		currentNpcCount++;
	}

	private void OnNpcDeath(Character character)
	{
		currentNpcCount--;
		if(currentNpcCount <= 0)
		{
			SpawnNewNPC();
		}
	}

	// end
}
