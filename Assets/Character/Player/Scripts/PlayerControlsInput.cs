using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsInput : MonoBehaviour
{
    PlayerInput inputManager;
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

    [field: SerializeField, Header("buffer")]
    public List<InputProperties> inputsQueue { get; private set; }
	[field: SerializeField]
	private float inputRemovalWait = 0.0f;

	private void Start()
	{
        inputManager = GetComponent<PlayerInput>();
	}

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
        ProcessJumpInput(val.isPressed);
    }
    private void ProcessJumpInput(bool inputState)
    {
        _JumpInput = inputState;
		if (inputManager.currentActionMap.name == "Gameplay")
		{
			InputProperties input = new InputProperties(InputProperties.InputType.jump);
			ProcessInput(input);
		}
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
        ProcessAttack(val.isPressed);
    }
    private void ProcessAttack(bool inputState)
    {
        _AttackInput = inputState;
        if (inputManager.currentActionMap.name  == "Gameplay")
        {
            InputProperties input = new InputProperties(InputProperties.InputType.attack);
            ProcessInput(input);
        }
    }
    public void UseAttack() => _AttackInput = false;
    public void OnSpecialAttack(InputValue val)
    {
        ProcessSpecial(val.isPressed);
	}
    private void ProcessSpecial(bool inputState)
    {
        _SpecialAttackInput = inputState;
        if (_SpecialAttackInput) _AttackInput = false;
		if (inputManager.currentActionMap.name == "Gameplay")
		{
			InputProperties input = new InputProperties(InputProperties.InputType.special);
			ProcessInput(input);
		}
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
        ProcessDefense(val.isPressed);
    }
    private void ProcessDefense(bool inputState)
    {
        _DefenseInput = inputState;
		if (inputManager.currentActionMap.name == "Gameplay")
		{
			InputProperties input = new InputProperties(InputProperties.InputType.defense);
			ProcessInput(input);
		}
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
				foreach (InputProperties inputProp in inputsQueue)
				{
                    inputProp.ResetPriority();
				}
				inputRemovalWait = 0.0f;
			}
			yield return null;
		}
	}

    public bool PollForSpecificInput(InputProperties.InputType inputType)
    {
        bool foundInput = false;

		if (inputsQueue.Count > 0) // make sure we have some inputs
		{
            foreach (InputProperties inputProp in inputsQueue)
            {
                if (inputType == inputProp.inputType)
                {
                    foundInput = true;
                    break;
                }
            }

		    inputsQueue.Clear(); // only clear if we have inputs
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
                if(inputProp.priority >  currentInputPriority)
                {
                    returnedInput = inputProp.inputType;
                    currentInputPriority = inputProp.priority;
                    // set the input and the priority to use for next time
                }
            }
            inputsQueue.Clear(); // only clear if we have inputs
		}

        return returnedInput;
    }

	// end
	#endregion
	// end
}
