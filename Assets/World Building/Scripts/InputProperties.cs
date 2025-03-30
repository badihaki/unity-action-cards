using System;

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
	public void ResetPriority() => priority = 0;
}
