using System.Collections.Generic;
using UnityEngine;

public class EnvChunk : MonoBehaviour
{
    [field: SerializeField]
    public List<Transform> pointsOfInterest { get; private set; }
    [field: SerializeField]
    public Transform playerSpawnPosition { get; private set; }

    void Awake()
    {
        playerSpawnPosition = transform.Find("PlayerSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
