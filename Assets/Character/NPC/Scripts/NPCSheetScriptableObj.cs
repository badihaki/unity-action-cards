using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Char", menuName = "Characters/NPC/New NPC")]
public class NPCSheetScriptableObj : CharacterSheet
{
    [Header("NPC Settings"), Tooltip("")]
    public List<NPCGroupScriptableObject> CharacterTypes;
}
