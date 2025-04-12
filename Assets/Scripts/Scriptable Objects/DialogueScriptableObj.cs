using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue", fileName = "dialogue_")]
public class DialogueScriptableObj : ScriptableObject
{
	public string speakingCharacterName;
	public string dialogueText;

	public bool hasChoice;
	[Serializable]
	public struct dialogueChoiceStruct
	{
		public string choiceText1;
		public string choiceText2;

		public dialogueChoiceStruct(string choiceText1, string choiceText2)
		{
			this.choiceText1 = choiceText1;
			this.choiceText2 = choiceText2;

			// quest given
		}
	}
	public dialogueChoiceStruct dialogueChoice;

	// end
}
