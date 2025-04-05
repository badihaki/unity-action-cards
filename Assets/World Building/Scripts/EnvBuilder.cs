using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class EnvBuilder : MonoBehaviour
{
	[field: SerializeField, Header("Env Layout")]
	private Transform layoutFolder;
	[field:SerializeField]
	private Transform floraFolder;
	[field: SerializeField]
	private Transform structuresFolder;
	[field:SerializeField]
	public List<EnvChunkScriptableObj> envChunkScrObjs { get; private set; }
    [SerializeField]
    private Vector2 gridLengthWidth;
	[SerializeField]
	private int maxLength = 5;
	[SerializeField]
	private int maxWidth = 6;
	[SerializeField]
    private int unitsToStep = 35;
    private Vector3 spawnPos;

	[field:SerializeField, Header("Points of Interest")]
	public List<Transform> usablePointsOfInterest { get; private set; } = new List<Transform>();

	[field:SerializeField, Header("Player stuff")]
	private List<Transform> possiblePlayerSpawnPoints = new List<Transform>();

	[field: SerializeField, Header("Nav Mesh")]
	private NavMeshSurface navMesh;

	[field: SerializeField, Header("NPCs")]
	private NPCSpawnManager npcSpawnManagerTemplate;
	[Serializable]
	private struct spawnManagerStruct
	{
		public NPCSpawnManager spawnManager;
		public List<NPCSheetScriptableObj> characters;
		public spawnManagerStruct(NPCSpawnManager spwnMng, List<NPCSheetScriptableObj> npcs)
		{
			spawnManager = spwnMng;
			characters = npcs;
		}
	}
	[SerializeField]
	private int npcSpawnPointsAllotted = 4;
	[SerializeField]
	private List<spawnManagerStruct> npcSpawnManagers = new List<spawnManagerStruct>();



	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        int length = UnityEngine.Random.Range(2, maxLength);
        int width = UnityEngine.Random.Range(2, maxWidth);
        gridLengthWidth = new Vector2(length, width);
		spawnPos = transform.position;
		navMesh = layoutFolder.GetComponent<NavMeshSurface>();
		npcSpawnManagers = new List<spawnManagerStruct>();
		BeginGeneration();
		GenerateNavMeshSurface();
		GenerateCharacterSpawnPoints();
    }

	private void BeginGeneration()
	{
		GenerateLevelLayout();
		PlaceCorruptionHeart();
		PlacePlayer();
	}

	#region Layout Generation
	private void GenerateLevelLayout()
	{
		GenerateColumns();
	}

	private void GenerateColumns()
	{
		// start with columns. for each column, generate rows
		for (int i = 0; i <= gridLengthWidth.y; i++)
		{
			GenerateRowInColumn(i);
			spawnPos.z -= unitsToStep;
		}
	}

	private void GenerateRowInColumn(int columnIndex)
	{
		for (int i = 0; i <= gridLengthWidth.x; i++)
		{
			if (columnIndex == 0)
			{
				GenerateNorthernChunks(columnIndex, i);
			}
			else if (columnIndex == gridLengthWidth.y)
			{
				GenerateSouthernChunks(columnIndex, i);
			}
			else if (i == 0)
			{
				GenerateWesternChunks(columnIndex, i);
			}
			else if(i == gridLengthWidth.x)
			{
				GenerateEasternChunks(columnIndex, i);
			}
			else
			{
				GenerateChunk(columnIndex, i);
			}
			spawnPos.x += unitsToStep;
		}
		ResetGridLength();
	}

	// Chunk generation is all about creating environment chunks
	#region Chunk Generation
	private void GenerateChunk(int columnIndex, int i)
	{
		List<EnvChunkScriptableObj> possibleChunks = new List<EnvChunkScriptableObj>();
		foreach (EnvChunkScriptableObj chunk in envChunkScrObjs)
		{
			if (!chunk.isBorder && !chunk.isCorner)
				possibleChunks.Add(chunk);
		}
		int chunkIndex = possibleChunks.Count > 1 ? UnityEngine.Random.Range(0, possibleChunks.Count) : 0;


		EnvChunk envChunk = Instantiate(possibleChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
		envChunk.name = $"Chunk-{columnIndex}-{i}";
		envChunk.transform.parent = layoutFolder;
		EnvChunkScriptableObj envChunkScrObjRef = null;
		envChunkScrObjRef = possibleChunks[chunkIndex];
		Transform[] pointsOfInterest = ExtractPointsOfInterest(envChunk);
		GenerateFlora(pointsOfInterest, envChunkScrObjRef.floraPropTemplates.ToArray());
	}

	private void GenerateEasternChunks(int columnIndex, int i)
	{
		List<EnvChunkScriptableObj> possibleChunks = new List<EnvChunkScriptableObj>();
		foreach (EnvChunkScriptableObj chunk in envChunkScrObjs)
		{
			if (chunk.isBorder && chunk.east)
				possibleChunks.Add(chunk);
		}
		int chunkIndex = possibleChunks.Count > 1 ? UnityEngine.Random.Range(0, possibleChunks.Count) : 0;

		EnvChunk envChunk = Instantiate(possibleChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
		envChunk.name = $"Chunk-EastBorder-{columnIndex}-{i}";
		envChunk.transform.parent = layoutFolder;
		EnvChunkScriptableObj envChunkScrObjRef = null;
		envChunkScrObjRef = possibleChunks[chunkIndex];
		Transform[] pointsOfInterest = ExtractPointsOfInterest(envChunk);
		GenerateFlora(pointsOfInterest, envChunkScrObjRef.floraPropTemplates.ToArray());
	}

	private void GenerateWesternChunks(int columnIndex, int i)
	{
		List<EnvChunkScriptableObj> possibleChunks = new List<EnvChunkScriptableObj>();
		foreach (EnvChunkScriptableObj chunk in envChunkScrObjs)
		{
			if (chunk.isBorder && chunk.west)
				possibleChunks.Add(chunk);
		}
		int chunkIndex = possibleChunks.Count > 1 ? UnityEngine.Random.Range(0, possibleChunks.Count) : 0;

		EnvChunk envChunk = Instantiate(possibleChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
		envChunk.name = $"Chunk-WestBorder-{columnIndex}-{i}";
		envChunk.transform.parent = layoutFolder;
		
		EnvChunkScriptableObj envChunkScrObjRef = null;
		envChunkScrObjRef = possibleChunks[chunkIndex];
		Transform[] pointsOfInterest = ExtractPointsOfInterest(envChunk);
		GenerateFlora(pointsOfInterest, envChunkScrObjRef.floraPropTemplates.ToArray());
	}

	private void GenerateSouthernChunks(int columnIndex, int i)
	{
		List<EnvChunkScriptableObj> possibleChunks = new List<EnvChunkScriptableObj>();
		List<EnvChunkScriptableObj> possibleEastCornerChunks = new List<EnvChunkScriptableObj>();
		List<EnvChunkScriptableObj> possibleWestCornerChunks = new List<EnvChunkScriptableObj>();

		EnvChunk envChunk;
		EnvChunkScriptableObj envChunkScrObjRef = null;

		foreach (EnvChunkScriptableObj chunk in envChunkScrObjs)
		{
			if (chunk.isBorder && chunk.south)
				possibleChunks.Add(chunk);
			else if(chunk.south && chunk.east && chunk.isCorner)
				possibleEastCornerChunks.Add(chunk);
			else if(chunk.south && chunk.west && chunk.isCorner)
				possibleWestCornerChunks.Add(chunk);
		}
		int chunkIndex = possibleChunks.Count > 1 ? UnityEngine.Random.Range(0, possibleChunks.Count) : 0;

		if (i == 0)
		{
			envChunk = Instantiate(possibleWestCornerChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunkScrObjRef = possibleWestCornerChunks[chunkIndex];
			envChunk.name = $"Chunk-SouthWestCorner-{columnIndex}-{i}";
		}
		else if (i == gridLengthWidth.x)
		{
			envChunk = Instantiate(possibleEastCornerChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunkScrObjRef = possibleEastCornerChunks[chunkIndex];
			envChunk.name = $"Chunk-SouthEastCorner-{columnIndex}-{i}";
		}
		else
		{
			envChunk = Instantiate(possibleChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunkScrObjRef = possibleChunks[chunkIndex];
			envChunk.name = $"Chunk-SouthBorder-{columnIndex}-{i}";
		}
		envChunk.transform.parent = layoutFolder;
		Transform[] pointsOfInterest = ExtractPointsOfInterest(envChunk);
		GenerateFlora(pointsOfInterest, envChunkScrObjRef.floraPropTemplates.ToArray());
	}

	private void GenerateNorthernChunks(int columnIndex, int i)
	{
		List<EnvChunkScriptableObj> possibleChunks = new List<EnvChunkScriptableObj>();
		List<EnvChunkScriptableObj> possibleEastCornerChunks = new List<EnvChunkScriptableObj>();
		List<EnvChunkScriptableObj> possibleWestCornerChunks = new List<EnvChunkScriptableObj>();

		EnvChunk envChunk;
		EnvChunkScriptableObj envChunkScrObjRef = null;

		foreach (EnvChunkScriptableObj chunk in envChunkScrObjs)
		{
			if (chunk.isBorder && chunk.north)
				possibleChunks.Add(chunk);
			else if (chunk.north && chunk.east && chunk.isCorner)
				possibleEastCornerChunks.Add(chunk);
			else if (chunk.north && chunk.west && chunk.isCorner)
				possibleWestCornerChunks.Add(chunk);
		}
		int chunkIndex = possibleChunks.Count > 1 ? UnityEngine.Random.Range(0, possibleChunks.Count) : 0;

		if (i == 0)
		{
			envChunk = Instantiate(possibleWestCornerChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunkScrObjRef = possibleWestCornerChunks[chunkIndex];
			envChunk.name = $"Chunk-NorthWestCorner-{columnIndex}-{i}";
		}
		else if (i == gridLengthWidth.x)
		{
			envChunk = Instantiate(possibleEastCornerChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunkScrObjRef = possibleEastCornerChunks[chunkIndex];
			envChunk.name = $"Chunk-NorthEastCorner-{columnIndex}-{i}";
		}
		else
		{
			envChunk = Instantiate(possibleChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunkScrObjRef = possibleChunks[chunkIndex];
			envChunk.name = $"Chunk-NorthBorder-{columnIndex}-{i}";
		}
		envChunk.transform.parent = layoutFolder;
		Transform[] pointsOfInterest = ExtractPointsOfInterest(envChunk);
		GenerateFlora(pointsOfInterest, envChunkScrObjRef.floraPropTemplates.ToArray());
	}
	#endregion

	#region Chunk Transform Extraction
	private Transform[] ExtractPointsOfInterest(EnvChunk envChunk)
	{
		Transform[] chunks = new Transform[envChunk.pointsOfInterest.Count];
		for (int i = 0; i < envChunk.pointsOfInterest.Count; i++)
		{
			usablePointsOfInterest.Add(envChunk.pointsOfInterest[i]);
			chunks[i] = envChunk.pointsOfInterest[i];
		}

		ExtractPlayerSpawnPos(envChunk);
		return chunks;
	}

	private void ExtractPlayerSpawnPos(EnvChunk envChunk)
	{
		possiblePlayerSpawnPoints.Add(envChunk.playerSpawnPosition);
	}
	#endregion

	#region Flora Generation
	private void GenerateFlora(Transform[] usableFloraLocations, PropScriptableObj[] flora)
	{
		foreach (Transform placementLocation in usableFloraLocations)
		{
			bool willBeUsed = GameManagerMaster.GameMaster.Dice.RollD10() > 5;

			if (willBeUsed)
			{
				int floraPropIndex = flora.Length > 1 ? UnityEngine.Random.Range(0, flora.Length - 1) : 0;
				//print($"flora prop location was {placementLocation.position.ToString()}");
				GameObject instantiatedFlora = Instantiate(flora[floraPropIndex].propGameObject, placementLocation.position, Quaternion.identity);
				instantiatedFlora.transform.parent = floraFolder;
				int removalIndex = usablePointsOfInterest.IndexOf(usablePointsOfInterest.Find(t => t == placementLocation));
				//print($"usable location was {usablePointsOfInterest[removalIndex].position.ToString()}");
				usablePointsOfInterest.RemoveAt(removalIndex);
				PlaceObjectOnNearestGround(instantiatedFlora);
			}
		}
	}
	#endregion

	private void PlaceObjectOnNearestGround(GameObject instantiatedObj)
	{
		Ray ray = new Ray(instantiatedObj.transform.position, Vector3.down);
		if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
		{
			Vector3 placementPos = hit.point;
			instantiatedObj.transform.position = placementPos;
		}
	}

	private void ResetGridLength()
	{
		int gridLength = (int)gridLengthWidth.x;
		int resetAmount = unitsToStep * (gridLength + 1);
		spawnPos.x -= resetAmount;
	}
	#endregion

	#region Corruption Heart
	private void PlaceCorruptionHeart()
	{
		int heartPlacementIndex = UnityEngine.Random.Range(0, usablePointsOfInterest.Count - 1);
		CorruptionHeart corruptionHeart = Instantiate(GameManagerMaster.GameMaster.Resources.corruptionHeart, usablePointsOfInterest[heartPlacementIndex].position, Quaternion.identity);
		usablePointsOfInterest.RemoveAt(heartPlacementIndex);
		corruptionHeart.name = "Corruption Heart";
		corruptionHeart.transform.parent = transform;
		Vector3 pos = corruptionHeart.transform.position;
		pos.y = pos.y + 0.76f;
		corruptionHeart.transform.position = pos;
		corruptionHeart.OnCorruptionHeartDestroyed += OnCorruptionHeartDestroyed;
	}

	private void OnCorruptionHeartDestroyed(CorruptionHeart destroyedHeart)
	{
		destroyedHeart.OnCorruptionHeartDestroyed -= OnCorruptionHeartDestroyed;
	}
	#endregion

	#region Characters
	private void PlacePlayer()
	{
		GameObject.Find("Player").TryGetComponent(out PlayerCharacter player);
		if (player != null)
		{
			int placementIndex = UnityEngine.Random.Range(0, possiblePlayerSpawnPoints.Count);
			print($"placement index is {placementIndex}");
			
			Vector3 placement = possiblePlayerSpawnPoints[placementIndex].position;
			placement.y = placement.y + 1;

			//print($"placing player at {placement.ToString()}");
			CharacterController charControl = player._PlayerActor.GetComponent<CharacterController>();
			charControl.enabled = false;
			player._PlayerActor.transform.position = placement;
			charControl.enabled = true;
		}
	}

	private void GenerateCharacterSpawnPoints()
	{
		int spawnPointsCreated = 0;
		spawnPointsCreated = GenerateAllSpawnManagers(spawnPointsCreated);
		for (int i = 0; i < npcSpawnManagers.Count; i++)
		{
			npcSpawnManagers[i].spawnManager.Initialize(npcSpawnManagers[i].characters);
		}
	}

	private int GenerateAllSpawnManagers(int spawnPointsCreated)
	{
		for (int i = 0; i < usablePointsOfInterest.Count; i++)
		{
			int roll = GameManagerMaster.GameMaster.Dice.RollD100();
			int rollModifier = (npcSpawnPointsAllotted - spawnPointsCreated) * 10;
			roll += rollModifier;
			print($"generating characters, roll modifier is {rollModifier} || spawn points allocated({npcSpawnPointsAllotted}) - spawn points created({spawnPointsCreated})");
			print($"full roll is {roll}");
			if (roll > 78)
			{
				GenerateSingleSpawnManager(i);
				spawnPointsCreated++;
				if (spawnPointsCreated >= npcSpawnPointsAllotted)
					break;
			}
		}

		return spawnPointsCreated;
	}

	private void GenerateSingleSpawnManager(int i)
	{
		print($"generating spawn point at usable POI(index-{i}) position");
		NPCSpawnManager spawnManager = Instantiate(npcSpawnManagerTemplate, usablePointsOfInterest[i].position, Quaternion.identity);
		spawnManager.transform.parent = layoutFolder;

		int chunkIndex = UnityEngine.Random.Range(0, envChunkScrObjs.Count);
		List<NPCSheetScriptableObj> spawnableNpcs = ExtractCharacters(chunkIndex);


		//spawnManagerStruct spawnManagerStruct = new spawnManagerStruct(spawnManager, spawnableNpcs);
		//spawnManagers.Add(spawnManagerStruct);
		npcSpawnManagers.Add(new spawnManagerStruct(spawnManager, spawnableNpcs));
	}

	private List<NPCSheetScriptableObj> ExtractCharacters(int chunkIndex)
	{
		bool willSpawnEnemies = GameManagerMaster.GameMaster.Dice.RollD100() > 50;
		bool canSpawnSpecial = GameManagerMaster.GameMaster.Dice.RollD100() > 90;
		List<NPCSheetScriptableObj> returnedCharactersList = new List<NPCSheetScriptableObj>();
		if (willSpawnEnemies)
		{
			print("adding enemies to the list");
			// spawn npcs from enemy list
			for(int regIndex = 0; regIndex < envChunkScrObjs[chunkIndex].enemies.Count; regIndex++)
			{
				returnedCharactersList.Add(envChunkScrObjs[chunkIndex].enemies[regIndex]);
				// check if can do specials, if so, add special enemies
				if(canSpawnSpecial)
				{
					print("adding SPECIAL enemies to the list");
					for (int specIndex = 0; specIndex < envChunkScrObjs[chunkIndex].specialEnemies.Count; specIndex++) 
					{
						returnedCharactersList.Add(envChunkScrObjs[chunkIndex].specialEnemies[specIndex]);
					}
				}
			}
		}
		else
		{
			// spawn regular npcs
			for (int regIndex = 0; regIndex < envChunkScrObjs[chunkIndex].regularNPCs.Count; regIndex++)
			{
				print("adding enemies to the list");
				returnedCharactersList.Add(envChunkScrObjs[chunkIndex].regularNPCs[regIndex]);
				// check if can do specials, if so, add special enemies
				if (canSpawnSpecial)
				{
					print("adding SPECIAL enemies to the list");
					for (int specIndex = 0; specIndex < envChunkScrObjs[chunkIndex].specialNPCs.Count; specIndex++)
					{
						returnedCharactersList.Add(envChunkScrObjs[chunkIndex].specialNPCs[specIndex]);
					}
				}
			}
			// same as before, but special npcs
		}
		return returnedCharactersList;
	}
	#endregion


	#region Navigation
	private void GenerateNavMeshSurface()
	{
		navMesh.BuildNavMesh();
	}

	public void UpdateNavMesh() => navMesh.UpdateNavMesh(navMesh.navMeshData);
	#endregion

	// end
}
