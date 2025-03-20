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
	[field: SerializeField] private List<NPCSheetScriptableObj> spawnableCharacters;
    [field: SerializeField] private int maxNumberOfSpawns = 4;
    [field: SerializeField] private int currentNpcCount = 0;
	//private WaitForSeconds spawnNpcWait = new WaitForSeconds(8.5f);
	[field: SerializeField] private float spawnTimer = 0.0f;
	[field: SerializeField] private float spawnTimerThreshold = 12.35f;
	[field: SerializeField] private float minSpawnTime = 6.35f;
	[field: SerializeField] private float maxSpawnTime = 18.78f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		SpawnNewNPC();
		RandomizeSpawnThreshold();
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
				ResetSpawnTimer();
			}
		}
	}

	private void ResetSpawnTimer()
	{
		RandomizeSpawnThreshold();
		spawnTimer = 0.0f;
	}
	//public float percentageNpcCount;
	private void RandomizeSpawnThreshold()
	{
		float newTimeThreshold = Random.Range(minSpawnTime, maxSpawnTime);
		float percentageNpcCount = ((float)currentNpcCount / maxNumberOfSpawns); // should be a value between 0 and 1
		print($"current {currentNpcCount} / max {maxNumberOfSpawns} is {percentageNpcCount}");
		if (percentageNpcCount > 0.3f)
			newTimeThreshold *= 2.35f;
		else if (percentageNpcCount > 0.7f)
			newTimeThreshold *= 3.75f;
			spawnTimerThreshold = newTimeThreshold;
	}

	private void SpawnNewNPC()
	{
		//NonPlayerCharacter activeNpc = Instantiate(npc, transform.position, Quaternion.identity);
		int spawnIndex = Random.Range(0, spawnableCharacters.Count);
		GameObject pooledNpc = ObjectPoolManager.GetObjectFromPool(GameManagerMaster.GameMaster.Resources.npcTemplate.gameObject, transform.position, Quaternion.identity, ObjectPoolManager.PoolFolder.Character, spawnableCharacters[spawnIndex]._CharacterName);
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
