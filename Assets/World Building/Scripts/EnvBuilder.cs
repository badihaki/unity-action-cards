using System.Collections.Generic;
using UnityEngine;

public class EnvBuilder : MonoBehaviour
{
    [field: SerializeField, Header("Env Layout")]
    public List<EnvChunkScriptableObj> envChunkScrObjs { get; private set; }
    [SerializeField]
    private Vector2 gridLengthWidth;
    [SerializeField]
    private int unitsToStep = 35;
    private Vector3 spawnPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int length = Random.Range(2, 6);
        int width = Random.Range(2, 9);
        gridLengthWidth = new Vector2(length, width);
		spawnPos = transform.position;
		BeginGeneration();
    }

	private void BeginGeneration()
	{
		GenerateColumns();
	}

	private void GenerateColumns()
	{
		// start with columns. for each column, generate rows
		for (int i = 0; i < gridLengthWidth.y; i++)
		{
			GenerateRowInColumn();
			spawnPos.z += unitsToStep;
		}
	}

	private void GenerateRowInColumn()
	{
		for (int i = 0; i < gridLengthWidth.x; i++)
		{
			print($"we are at index {i} trying to get to {gridLengthWidth.x}");
			Instantiate(envChunkScrObjs[0].chunkGameObj, spawnPos, Quaternion.identity);
			spawnPos.x += unitsToStep;
		}
		int gridLength = (int)gridLengthWidth.x;
		print("got grid lenght");
		int resetAmount = unitsToStep * gridLength;
		print("got grid reset amount");
		spawnPos.x -= resetAmount;
		print("resetting");
	}

	// end
}
