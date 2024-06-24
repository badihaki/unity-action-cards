using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private Character Character;
    [field: SerializeField] public Animator animationController { get; protected set; }

    [field: SerializeField, Header("Animator Movement")]
    public Vector3 animatorMovementVector { get; protected set; } = new Vector3();
    [field: SerializeField] protected bool controlByRootMotion = false;

    public void Initialize(Character character)
    {
        Character = character;
        animationController = GetComponent<Animator>();
    }
}
