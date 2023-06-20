using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Char", menuName = "Characters/New Character")]
public class CharacterSheet : ScriptableObject
{
    [Header("Base Information")]
    [Tooltip("The name of this character")]
    public string _CharacterName;
    [Tooltip("The starting health of this character")]
    public int _StartingHealth = 1;

    [Header("Mechanical Info")]
    [Tooltip("The walking speed the character moves at")]
    public float _WalkSpeed = 5.00f;
    [Tooltip("The running speed the character moves at")]
    public float _RunSpeed = 8.00f;
    [Tooltip("The speed at which the character dashes/rushes")]
    public float _RushDashSpeed = 15.65f;
    [Tooltip("Yep, how far the character can jump??")]
    public float _JumpPower = 5.25f;
}
