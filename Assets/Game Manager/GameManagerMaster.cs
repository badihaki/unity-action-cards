using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMaster : MonoBehaviour
{
    public static GameManagerMaster GameMaster { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (GameMaster != null && GameMaster != this)
        {
            Debug.LogError("More than 1 Game Master Detected");
            Debug.LogWarning("There can be only one!!");
            Destroy(gameObject);
        }
        else
        {
            GameMaster = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
