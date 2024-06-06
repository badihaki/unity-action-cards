using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : Character, IDestroyable
{
    [field: SerializeField] public NPCMovementController _MoveController { get; private set; }
    public void DestroyEntity()
    {
        print("goodbye, " + name);
        Destroy(gameObject);
    }

    public override void Initialize()
    {
        base.Initialize();

        _MoveController = GetComponent<NPCMovementController>();
        _MoveController.InitializeNPCMovement(this);
    }
}
