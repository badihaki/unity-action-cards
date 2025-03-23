using System.Collections.Generic;
using UnityEngine;

public class EnvBuilder : MonoBehaviour
{
    [field: SerializeField, Header("Env Layout")]
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int length = Random.Range(2, maxLength);
        int width = Random.Range(2, maxWidth);
        gridLengthWidth = new Vector2(length, width);
		spawnPos = transform.position;
		BeginGeneration();
    }

	private void BeginGeneration()
	{
		GenerateLevelLayout();
	}

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
			print($"we are at index {i} trying to get to {gridLengthWidth.x}");
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

	private void GenerateChunk(int columnIndex, int i)
	{
		List<EnvChunkScriptableObj> possibleChunks = new List<EnvChunkScriptableObj>();
		foreach (EnvChunkScriptableObj chunk in envChunkScrObjs)
		{
			if(!chunk.isBorder && !chunk.isCorner)
				possibleChunks.Add(chunk);
		}
		int chunkIndex = possibleChunks.Count > 1 ? Random.Range(0, possibleChunks.Count) : 0;
		

		EnvChunk envChunk = Instantiate(possibleChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
		envChunk.name = $"Chunk-{columnIndex}-{i}";
		envChunk.transform.parent = transform;
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
		envChunk.transform.parent = transform;
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
		envChunk.transform.parent = transform;
	}

	private void GenerateSouthernChunks(int columnIndex, int i)
	{
		List<EnvChunkScriptableObj> possibleChunks = new List<EnvChunkScriptableObj>();
		List<EnvChunkScriptableObj> possibleEastCornerChunks = new List<EnvChunkScriptableObj>();
		List<EnvChunkScriptableObj> possibleWestCornerChunks = new List<EnvChunkScriptableObj>();

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

		EnvChunk envChunk;

		if (i == 0)
		{
			envChunk = Instantiate(possibleWestCornerChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunk.name = $"Chunk-SouthWestCorner-{columnIndex}-{i}";
		}
		else if (i == gridLengthWidth.x)
		{
			envChunk = Instantiate(possibleEastCornerChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunk.name = $"Chunk-SouthEastCorner-{columnIndex}-{i}";
		}
		else
		{
			envChunk = Instantiate(possibleChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunk.name = $"Chunk-SouthBorder-{columnIndex}-{i}";
		}
		envChunk.transform.parent = transform;
	}

	private void GenerateNorthernChunks(int columnIndex, int i)
	{
		List<EnvChunkScriptableObj> possibleChunks = new List<EnvChunkScriptableObj>();
		List<EnvChunkScriptableObj> possibleEastCornerChunks = new List<EnvChunkScriptableObj>();
		List<EnvChunkScriptableObj> possibleWestCornerChunks = new List<EnvChunkScriptableObj>();

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

		EnvChunk envChunk;

		if (i == 0)
		{
			envChunk = Instantiate(possibleWestCornerChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunk.name = $"Chunk-NorthWestCorner-{columnIndex}-{i}";
		}
		else if (i == gridLengthWidth.x)
		{
			envChunk = Instantiate(possibleEastCornerChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunk.name = $"Chunk-NorthEastCorner-{columnIndex}-{i}";
		}
		else
		{
			envChunk = Instantiate(possibleChunks[chunkIndex].chunkGameObj, spawnPos, Quaternion.identity);
			envChunk.name = $"Chunk-NorthBorder-{columnIndex}-{i}";
		}
		envChunk.transform.parent = transform;
	}

	private void ResetGridLength()
	{
		int gridLength = (int)gridLengthWidth.x;
		int resetAmount = unitsToStep * (gridLength + 1);
		spawnPos.x -= resetAmount;
	}

	// end
}
