using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Char", menuName = "Characters/New Character")]
public class CharacterSheet : ScriptableObject
{
    [Header("Base Information")]
    [Tooltip("The name of this character")]
    public string CharacterName;
    [Tooltip("The starting health of this character")]
    public int StartingHealth = 1;

    [Header("Mechanical Info")]
    public float Speed = 5.00f;
}
