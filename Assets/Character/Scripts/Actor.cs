using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private Character Character;

    public void Initialize(Character character)
    {
        Character = character;
    }
}
