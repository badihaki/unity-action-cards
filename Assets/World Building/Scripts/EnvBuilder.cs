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



	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        int length = Random.Range(2, maxLength);
        int width = Random.Range(2, maxWidth);
        gridLengthWidth = new Vector2(length, width);
		spawnPos = transform.position;
		navMesh = layoutFolder.GetComponent<NavMeshSurface>();
		BeginGeneration();
		GenerateNavMeshSurface();
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
		int chunkIndex = possibleChunks.Count > 1 ? Random.Range(0, possibleChunks.Count) : 0;


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
		int chunkIndex = possibleChunks.Count > 1 ? Random.Range(0, possibleChunks.Count) : 0;

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
		int chunkIndex = possibleChunks.Count > 1 ? Random.Range(0, possibleChunks.Count) : 0;

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
		int chunkIndex = possibleChunks.Count > 1 ? Random.Range(0, possibleChunks.Count) : 0;

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
		int chunkIndex = possibleChunks.Count > 1 ? Random.Range(0, possibleChunks.Count) : 0;

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
				int floraPropIndex = flora.Length > 1 ? Random.Range(0, flora.Length - 1) : 0;
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
		int heartPlacementIndex = Random.Range(0, usablePointsOfInterest.Count - 1);
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

	private void PlacePlayer()
	{
		GameObject.Find("Player").TryGetComponent(out PlayerCharacter player);
		if (player != null)
		{
			int placementIndex = Random.Range(0, possiblePlayerSpawnPoints.Count);
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

	private void GenerateNavMeshSurface()
	{
		navMesh.BuildNavMesh();
	}

	public void UpdateNavMesh() => navMesh.UpdateNavMesh(navMesh.navMeshData);

	// end
}
