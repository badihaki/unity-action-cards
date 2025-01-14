using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "npc_", menuName = "Characters/NPC/New NPC")]
public class NPCSheetScriptableObj : CharacterSheet
{
    [Header("NPC Settings"), Tooltip("The different groups this NPC can belong to")]
    public List<NPCGroupScriptableObject> CharacterTypes;
    [field: SerializeField, Tooltip("The actor used as the physical, in-game representation of this NPC")]
    public GameObject Actor;
    [field:SerializeField, Tooltip("If true, this entity will be aggressive to any character that doesn't belong to it's Group.")]
    public bool isAlwaysAggressive { get; private set; }
}
