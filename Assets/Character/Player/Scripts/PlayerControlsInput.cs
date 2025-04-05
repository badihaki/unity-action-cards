using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerControlsInput : MonoBehaviour
{
    [SerializeField]
    PlayerInput inputManager;

    private enum InputMaps
    {
        Combat = 0,
        UI = 1,
        Spellsling = 2
    }
    [SerializeField]
    private InputMaps currentInputMap = 0;

    [field: SerializeField] public Vector2 _MoveInput { get; private set; }
    [field: SerializeField] public Vector2 _AimInput { get; private set; }
    [field: SerializeField] public bool _LockOnInput { get; private set; }
    [field: SerializeField] public bool _JumpInput { get; private set; }
    [field: SerializeField] public bool _RushInput { get; private set; }
    [field: SerializeField] public bool _CardsInput { get; private set; }
    [field: SerializeField] public bool _SpellslingInput { get; private set; }
    [field: SerializeField] public bool _AttackInput { get; private set; }
    [field: SerializeField] public bool _SpecialAttackInput { get; private set; }
    [field: SerializeField] public bool _InteractInput { get; private set; }
    [field: SerializeField] public bool _DefenseInput { get; private set; }
	[field: SerializeField] public int _SelectSpellInput { get; private set; }
    [field: SerializeField] public bool _UiCancelInput { get; private set; }

    [field: SerializeField, Header("buffer")]
    public List<InputProperties> inputsQueue { get; private set; }
	[field: SerializeField]
	private float inputRemovalTimer = 0.0f;
    [field: SerializeField]
    private float inputRemovalMaxTime = 0.15f;
    private WaitForSeconds inputRemovalWaitTime = new WaitForSeconds(0.25f);

	private void Start()
	{
        inputManager = GetComponent<PlayerInput>();
        SetInputMap(0);
	}

	#region Input Map stuff
    public void SetInputMap(int inputMapId)
    {
        switch(inputMapId)
        {
            case 0:
                currentInputMap = InputMaps.Combat;
				inputManager.SwitchCurrentActionMap("Combat");
				break;
            case 1:
                currentInputMap = InputMaps.UI;
				inputManager.SwitchCurrentActionMap("UI");
				break;
            case 2:
                currentInputMap = InputMaps.Spellsling;
				inputManager.SwitchCurrentActionMap("Spellsling");
				break;
		}
    }
	#endregion

	#region Get and Process Inputs
	public void OnMove(InputValue val)
    {
        ProcessMoveInput(val.Get<Vector2>());
    }
    private void ProcessMoveInput(Vector2 input)
    {
        Vector2 moveVector = input;
        if (moveVector.x > 0) moveVector.x = 1;
        else if (moveVector.x < 0) moveVector.x = -1;
        if (moveVector.y > 0) moveVector.y = 1;
        else if (moveVector.y < 0) moveVector.y = -1;
        _MoveInput = moveVector;
    }
    public void OnAim(InputValue val)
    {
        ProcessAimInput(val.Get<Vector2>());
    }
    private void ProcessAimInput(Vector2 input)
    {
        _AimInput = input.normalized;
    }

    public void OnLockOn(InputValue val)
    {
        ProcessLockOnInput(val.isPressed);
    }
    private void ProcessLockOnInput(bool inputState) => _LockOnInput = inputState;

    public void OnJump(InputValue val)
    {
		if (inputManager.currentActionMap.name == "Combat")
		{
			InputProperties input = new InputProperties(InputProperties.InputType.jump);
			ProcessInput(input);
		}
        else
            ProcessJumpInput(val.isPressed);
    }
    private void ProcessJumpInput(bool inputState)
    {
        _JumpInput = inputState;
	}
    public void UseJump() => _JumpInput = false;

    public void OnRush(InputValue val)
    {
        ProcessRushInput(val.isPressed);
    }

    private void ProcessRushInput(bool inputState)
    {
        _RushInput = inputState;
    }
    public void UseRush() => _RushInput = false;
    public void OnCards(InputValue val)
    {
        ProcessCards(val.isPressed);
    }
    private void ProcessCards(bool inputState)
    {
        _CardsInput = inputState;
    }
    public void CardSelected() => _CardsInput = false;

    public void OnAttack(InputValue val)
    {
        if (currentInputMap == 0)
        {
            InputProperties input = new InputProperties(InputProperties.InputType.attack);
            ProcessInput(input);
        }
        else if (currentInputMap == InputMaps.Spellsling)
        {
            ProcessAttack(val.isPressed);
        }
        else
            ProcessAttack(val.isPressed);
    }
    private void ProcessAttack(bool inputState)
    {
        _AttackInput = inputState;
    }
    public void UseAttack() => _AttackInput = false;
    public void OnSpecialAttack(InputValue val)
    {
		if (inputManager.currentActionMap.name == "Combat")
		{
			print("special attacking from combat action map");
			InputProperties input = new InputProperties(InputProperties.InputType.special);
			ProcessInput(input);
		}
        else
        {
            ProcessSpecial(val.isPressed);
        }
	}
    private void ProcessSpecial(bool inputState)
    {
        _SpecialAttackInput = inputState;
        if (_SpecialAttackInput) _AttackInput = false;
	}
    public void UseSpecialAttack() => _SpecialAttackInput = false;

    public void OnSpell(InputValue val)
    {
        ProcessSpellsling(val.isPressed);
    }
    private void ProcessSpellsling(bool inputState)
    {
        _SpellslingInput = inputState;
    }
    public void UseSpell() => _SpellslingInput = false;
    
    public void OnInteract(InputValue val)
    {
        ProcessInteract(val.isPressed);
    }
    private void ProcessInteract(bool inputState)
    {
        _InteractInput = inputState;
    }
    public void UseInteract() => _InteractInput = false;

    public void OnDefenseAction(InputValue val)
    {
		if (inputManager.currentActionMap.name == "Combat")
		{
			InputProperties input = new InputProperties(InputProperties.InputType.defense);
			ProcessInput(input);
		}

        ProcessDefense(val.isPressed);
    }
    private void ProcessDefense(bool inputState)
    {
        _DefenseInput = inputState;
	}
    public void UseDefense()
    {
        _DefenseInput = false;
    }

    public void OnSelectSpell(InputValue val)
    {
        float value = val.Get<float>();
        _SelectSpellInput = value > 0 ? 1 : -1;
    }
    public void ResetSelectSpell() => _SelectSpellInput = 0;

    public void OnQuit(InputValue val)
    {
        if (val.isPressed)
            GameManagerMaster.GameMaster.QuitGame();
    }

    public void OnCancel(InputValue val)
    {
        if (val.isPressed)
			ProcessUiCancel(val.isPressed);
    }
	private void ProcessUiCancel(bool inputState)
	{
        _UiCancelInput = inputState;
	}
    public void ResetUiCancel() => _UiCancelInput = false;
	#endregion


	#region input buffer
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
		print($"adding input {input.inputType.ToString()} to the queue");
		inputsQueue.Insert(0, input);
		RemovalOldInputs();
		//print($"added input {inputsQueue[0].inputType.ToString()}");
		if (inputRemovalTimer <= 0)
		{
			StartCoroutine(ManageQueue());
		}
	}

	private void RemovalOldInputs()
	{
		if (inputsQueue.Count > 4)
		{
            print($"removing input {inputsQueue[inputsQueue.Count - 1].inputType.ToString()} to the queue");

			inputsQueue.RemoveAt(inputsQueue.Count - 1);
		}
	}

	private IEnumerator ManageQueue()
	{
        print("starting management of queue");

        while (inputsQueue.Count > 0)
        {
            yield return inputRemovalWaitTime;

            if (inputsQueue.Count > 0)
            {
                inputsQueue.RemoveAt(0);
                foreach (InputProperties inputProp in inputsQueue)
                {
                    inputProp.ResetPriority();
                }
            }
		}

	}

	public bool PollForSpecificInput(InputProperties.InputType inputType)
    {
        bool foundInput = false;

		if (inputsQueue.Count > 0) // make sure we have some inputs
		{
            for (int i = 0; i < inputsQueue.Count; i++)
            {
                if (inputsQueue[i].inputType == inputType)
                {
                    print($"taking input from input queue position - {i} - ");
                    print($"input is {inputsQueue[i].inputType.ToString()}");
					foundInput = true;
                    inputsQueue.RemoveAt(i);
                    break;
                }
            }
		}

		return foundInput;
    }

    public InputProperties.InputType PollForDesiredInput()
    {
        InputProperties.InputType returnedInput = InputProperties.InputType.None;
        int currentInputPriority = 0;

        if(inputsQueue.Count > 0) // make sure we have some inputs
        {
            // loop through to see if the priority is greater than our stored priority
            foreach (InputProperties inputProp in inputsQueue)
            {
                if (returnedInput != InputProperties.InputType.None)
                {
                    if (inputProp.priority > currentInputPriority)
                    {
                        returnedInput = inputProp.inputType;
                        currentInputPriority = inputProp.priority;
                        // set the input and the priority to use for next time
                    }

                }
                else // returned input is none, but there are inputs, so this input becomes the returned input
                    returnedInput = inputProp.inputType;
            }
            inputRemovalTimer = 0.0f;
            inputsQueue.Clear(); // only clear if we have inputs
		}

        return returnedInput;
    }

	// end
	#endregion
	// end
}
