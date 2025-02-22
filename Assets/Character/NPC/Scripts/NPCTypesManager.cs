using System.Collections.Generic;
using UnityEngine;

public class NPCTypesManager : MonoBehaviour
{
	[field:SerializeField]
	public List<NPCTypeScriptableObject> npcTypesList { get; private set; }
	public void Initialize(NPCSheetScriptableObj charSheet)
	{
		npcTypesList = new List<NPCTypeScriptableObject>(charSheet.CharacterTypes);
	}

	public bool SharesTypeWith(NonPlayerCharacter character)
	{
		foreach (var type in character._NPCharacterSheet.CharacterTypes)
		{
			if (npcTypesList.Contains(type))
				return true;
		}
		return false;
	}

	// end
}
