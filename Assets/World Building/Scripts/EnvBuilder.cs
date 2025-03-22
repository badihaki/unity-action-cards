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
			GameObject envChunk = Instantiate(envChunkScrObjs[0].chunkGameObj, spawnPos, Quaternion.identity);
			
			
			float value = envChunk.transform.Find("Ground").GetComponent<Collider>().bounds.size.x;
			print($"collider x size is {value} ");
			// confirmed, we can use this value to set for the size needed

			if (columnIndex == 0)
			{
				envChunk.name = envChunk.name + "-NorthBorder";
			}
			else if (columnIndex == gridLengthWidth.y)
			{
				envChunk.name = envChunk.name + "-SouthBorder";
			}
			if (i == 0)
			{
				envChunk.name = envChunk.name + "-WestBorder";
			}
			else if(i == gridLengthWidth.x)
			{
				envChunk.name = envChunk.name + "-EastBorder";
			}
			envChunk.transform.parent = transform;
			spawnPos.x += unitsToStep;
		}
		ResetGridLength();
	}

	private void ResetGridLength()
	{
		int gridLength = (int)gridLengthWidth.x;
		int resetAmount = unitsToStep * (gridLength + 1);
		spawnPos.x -= resetAmount;
	}

	// end
}
