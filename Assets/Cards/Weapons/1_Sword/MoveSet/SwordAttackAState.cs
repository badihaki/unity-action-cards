using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Attack A", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Attack A")]
public class SwordAttackAState : PlayerAttackSuperState
{
    public SwordAttackAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

	public override void EnterState()
	{
		base.EnterState();
		_PlayerCharacter._MoveController.ZeroOutVelocity();
		//_AttackController.SetAttackParameters(false, false);
		_AttackController.SetAttackParameters();
		_PlayerCharacter._WeaponController.SetShowWeapons(true);
	}

	public override void ExitState()
	{
		base.ExitState();
		//ShowOrHideWeapon(false);
	}

	public override void CheckStateTransitions()
	{
		if (canCombo)
		{
			//if (attackInput) _StateMachine.ChangeState(_AttackController._AttackB);
			//if (jumpInput) _StateMachine.ChangeState(_AttackController._LauncherAttack);
			//if (specialInput) _StateMachine.ChangeState(_AttackController._FinisherA);

			switch (_PlayerCharacter._Controls.PollForDesiredInput())
			{
				case InputProperties.InputType.jump:
					_PlayerCharacter._Controls.UseJump();
					_StateMachine.ChangeState(_AttackController._LauncherAttack);
					break;
				case InputProperties.InputType.attack:
					_PlayerCharacter._Controls.UseAttack();
					_StateMachine.ChangeState(_AttackController._AttackB);
					break;
				case InputProperties.InputType.special:
					_PlayerCharacter._Controls.UseSpecialAttack();
					_StateMachine.ChangeState(_AttackController._FinisherA);
					break;
				default:
					break;
			}
		}

		base.CheckStateTransitions();
	}

	public override void TriggerVisualEffect()
	{
		base.TriggerVisualEffect();
		// Vector3 position = new Vector3(_PlayerCharacter._Actor.transform.position.x, _PlayerCharacter._Actor.transform.position.y + 1.75f, _PlayerCharacter._Actor.transform.position.z + 0.75f);
		// Vector3 position = new Vector3(_PlayerCharacter._Actor.transform.position.x, _PlayerCharacter._AttackController._WeaponHolderR.transform.position.y, _PlayerCharacter._Actor.transform.position.z + 0.75f);
		// Quaternion rotation = Quaternion.Euler(_PlayerCharacter._PlayerActor.transform.forward);

		// GameObject vfx = Instantiate(_PlayerCharacter._AttackController._CurrentWeapon._WeaponAttackFX, position, rotation);
		// vfx.transform.rotation = _PlayerCharacter._PlayerActor.transform.rotation;
	}
}
