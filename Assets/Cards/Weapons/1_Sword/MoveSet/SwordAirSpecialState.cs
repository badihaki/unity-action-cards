using UnityEngine;

[CreateAssetMenu(fileName = "Sword Air Special", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Air Special")]
public class SwordAirSpecialState : PlayerAirCombatSuperState
{
	public SwordAirSpecialState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}


	private bool continued = false;

	public override void EnterState()
	{
		base.EnterState();

		continued = false;
		_PlayerCharacter._AnimationController.SetBool("continue", false);
		_PlayerCharacter._AnimationController.SetBool("continue2", false);

		//_AttackController.SetAttackParameters(true, true, 2);
		_AttackController.SetAttackParameters(responsesToDamage.knockBack, 2, 1.5f);
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (!continued && _PlayerCharacter._CheckGrounded.IsGrounded(0.5f))
		{
			continued = true;
			_PlayerCharacter._AnimationController.SetBool("continue", true);
			_PlayerCharacter._AnimationController.SetBool("continue2", true);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		if (canCombo)
		{
			_PlayerCharacter._LocomotionController.ApplyGravity(1.35f);
			_PlayerCharacter._LocomotionController.DetectMove(_PlayerCharacter._PlayerActor.transform.forward);
			_PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
		}
	}

	public override void TriggerSideEffect()
	{
		if (!canCombo)
		{
			base.TriggerSideEffect();
			_PlayerCharacter._AnimationController.SetBool("continue", true);
		}
	}

	public override void ExitState()
	{
		base.ExitState();

		_PlayerCharacter._AnimationController.SetBool("continue", false);
		_PlayerCharacter._AnimationController.SetBool("continue2", false);
		continued = false;
	}

	public override void CheckStateTransitions()
	{
		if (_AnimationIsFinished)
		{
			_StateMachine.ChangeState(_StateMachine._IdleState);
		}
	}
}
