using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Attack B", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Attack B")]
public class SwordAttackBState : PlayerAttackSuperState
{
    public SwordAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

	public override void EnterState()
	{
		base.EnterState();
		_PlayerCharacter._LocomotionController.ZeroOutVelocity();
		_PlayerCharacter._AttackController.SetAttackParameters(1.025f, 1.35f, 1);
		ShowOrHideWeapon(true);
	}

	public override void ExitState()
	{
		base.ExitState();
		//ShowOrHideWeapon(true);
	}

	public override void CheckStateTransitions()
	{
		if (canCombo && attackInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._AttackC);
		// if (canCombo && specialInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._FinisherA);
		// if (canCombo && jumpInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._LauncherAttack);

		base.CheckStateTransitions();
	}
	public override void TriggerVisualEffect()
	{
		base.TriggerVisualEffect();
		// Vector3 position = new Vector3(_PlayerCharacter._Actor.transform.position.x, _PlayerCharacter._Actor.transform.position.y + 1.75f, _PlayerCharacter._Actor.transform.position.z + 0.75f);
		//Vector3 position = new Vector3(_PlayerCharacter._Actor.transform.position.x, _PlayerCharacter._AttackController._WeaponHolderR.transform.position.y, _PlayerCharacter._Actor.transform.position.z + 0.75f);
		//Quaternion rotation = Quaternion.Euler(_PlayerCharacter._PlayerActor.transform.forward);

		//GameObject vfx = Instantiate(_PlayerCharacter._AttackController._CurrentWeapon._WeaponAttackFX, position, rotation);
		//vfx.transform.rotation = _PlayerCharacter._PlayerActor.transform.rotation;
	}
}
