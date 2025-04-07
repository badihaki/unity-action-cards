using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInteractionCanvas : InteractionCanvasBase
{
	[field: SerializeField, Header("Dialogue")]
	private GameObject dialoguePanel;
	private TextMeshProUGUI dialogueText;
	[SerializeField]
	private GameObject nextBtn;
	//private Button endBtn;
	[SerializeField]
	private int currentDialogueIndex = 0;
	private InteractionTalk talkInteraction;

	public override void Initialize(Interaction interaction, string interactionName)
	{
		base.Initialize(interaction, interactionName);

		dialoguePanel = transform.Find("DialoguePanel").gameObject;
		basePanel = transform.Find("BasePanel").gameObject;
		dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
		nextBtn = dialoguePanel.transform.Find("btn_DialogueNext").gameObject;
		talkInteraction = _Interaction.interactionScrObj as InteractionTalk;

		SetDialogue();
		
		dialoguePanel.SetActive(false);
	}

	private void SetDialogue()
	{
		dialogueText.text = talkInteraction.dialoguesList[currentDialogueIndex].dialogueText;
		CheckForNextDialogue(talkInteraction);
	}

	private void CheckForNextDialogue(InteractionTalk interaction)
	{
		if (interaction.dialoguesList.Count <= currentDialogueIndex + 1)
		{
			nextBtn.SetActive(false);
		}
	}

	public override void OnInteractBtnClilcked()
	{
		base.OnInteractBtnClilcked();
		basePanel.SetActive(false);
		dialoguePanel.SetActive(true);
		SetDialogue();
	}

	public override void CancelInteraction()
	{
		basePanel.SetActive(true);
		dialoguePanel.SetActive(false);
		currentDialogueIndex = 0;
		base.CancelInteraction();
	}

	public void NextDialogue()
	{
		currentDialogueIndex++;
		SetDialogue();
		//interaction.dialoguesList
	}


	// end
}
