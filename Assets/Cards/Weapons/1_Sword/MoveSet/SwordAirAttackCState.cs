using UnityEngine;

[CreateAssetMenu(fileName = "Sword Air Attack C", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Air Attack C")]
public class SwordAirAttackCState : PlayerAirCombatSuperState
{
	public SwordAirAttackCState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}
	public override void EnterState()
	{
		base.EnterState();

		//_AttackController.SetAttackParameters(true, true, 1);
		_AttackController.SetAttackParameters(responsesToDamage.launch, 1, 1.5f);
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (canCombo)
		{
			if (specialInput)
				_StateMachine.ChangeState(_AttackController._AirSpecial);
		}
	}

	public override void TriggerSideEffect()
	{
		_PlayerCharacter._LocomotionController.Jump(0.125f);
	}
}
