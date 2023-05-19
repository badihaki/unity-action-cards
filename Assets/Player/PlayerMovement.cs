using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player Player;
    private Rigidbody Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(Player controller)
    {
        Player = controller;
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCharacter(Vector2 direction)
    {
        //
    }
}
