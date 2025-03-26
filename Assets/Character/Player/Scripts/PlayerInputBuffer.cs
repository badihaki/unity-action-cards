using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using System.Collections;

public class PlayerInputBuffer : MonoBehaviour
{
	[field: SerializeField]
	public List<InputProperties> inputsQueue { get; private set; }
	[field: SerializeField]
	private float inputRemovalWait = 0.0f;

	public void OnJump(InputValue val)
	{
		if (val.isPressed)
		{
			InputProperties input = new InputProperties(InputProperties.InputType.jump);
			ProcessInput(input);
		}
	}

	public void OnAttack(InputValue val)
	{
		InputProperties input = new InputProperties(InputProperties.InputType.attack);
		ProcessInput(input);
	}

	public void OnSpecialAttack(InputValue val)
	{
		InputProperties input = new InputProperties(InputProperties.InputType.special);
		ProcessInput(input);
	}

	public void OnDefenseAction(InputValue val)
	{
		InputProperties input = new InputProperties(InputProperties.InputType.defense);
		ProcessInput(input);
	}

	private void ProcessInput(InputProperties input)
	{
		InputProperties loggedInput = inputsQueue.Find(inputProp => inputProp.inputType == input.inputType);
		if (loggedInput != null)
			loggedInput.AddPriority();
		else
			AddInputToQueue(input);
	}

	private void AddInputToQueue(InputProperties input)
	{
		inputsQueue.Insert(0, input);
		RemovalOldInputs();
		print($"added input {inputsQueue[0].inputType.ToString()}");
		if (inputRemovalWait == 0)
		{
			StartCoroutine(ManageQueue());
		}
	}

	private void RemovalOldInputs()
	{
		if (inputsQueue.Count > 4)
		{
			inputsQueue.RemoveAt(inputsQueue.Count - 1);
		}
	}

	private IEnumerator ManageQueue()
	{
		while (inputsQueue.Count > 0)
		{
			inputRemovalWait += Time.deltaTime;
			print($"removing input {inputsQueue[0].inputType.ToString()}");
			if (inputRemovalWait > 0.85f)
			{
				inputsQueue.RemoveAt(0);
				inputRemovalWait = 0.0f;
			}
			yield return null;
		}
	}

	// end
}

[Serializable]
public class InputProperties
{
	public enum InputType
	{
		None = 0,
		jump = 1,
		attack = 2,
		special = 3,
		defense = 4,
	}
	public InputType inputType;
	public int priority;

	public InputProperties(InputType _inputType)
	{
		inputType = _inputType;
		priority = 0;
	}

	public void AddPriority() => priority++;
}
