using UnityEngine;

public class PlayerCinematicSuperState : PlayerState
{
	public PlayerCinematicSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	protected bool interactInput;

	public override void CheckInputs()
	{
		base.CheckInputs();
		interactInput = _PlayerCharacter._Controls._InteractInput;
	}
}
