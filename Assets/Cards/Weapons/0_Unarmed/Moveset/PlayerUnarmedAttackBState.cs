using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Attack B", menuName = "Characters/Player/Create Attacks/00_Unarmed/Unarmed Attack B")]
public class PlayerUnarmedAttackBState : PlayerAttackSuperState
{
    public PlayerUnarmedAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _AttackController.SetAttackParameters(1.025f, 1.35f, 1);
        ShowOrHideWeapon(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        ShowOrHideWeapon(false);
    }

    public override void CheckStateTransitions()
    {
        if (canCombo && attackInput) _StateMachine.ChangeState(_AttackController._AttackC);
        if (canCombo && specialInput) _StateMachine.ChangeState(_AttackController._FinisherA);
        if (canCombo && jumpInput) _StateMachine.ChangeState(_AttackController._LauncherAttack);

        base.CheckStateTransitions();
    }
    public override void TriggerVisualEffect()
    {
        base.TriggerVisualEffect();
        // Vector3 position = new Vector3(_PlayerCharacter._Actor.transform.position.x, _PlayerCharacter._Actor.transform.position.y + 1.75f, _PlayerCharacter._Actor.transform.position.z + 0.75f);
        Vector3 position = new Vector3(_PlayerCharacter._Actor.transform.position.x, _PlayerCharacter._WeaponController._WeaponHolderR.transform.position.y, _PlayerCharacter._Actor.transform.position.z + 0.75f);
        Quaternion rotation = Quaternion.Euler(_PlayerCharacter._PlayerActor.transform.forward);

        GameObject vfx = Instantiate(_PlayerCharacter._WeaponController._CurrentWeapon._WeaponAttackFX, position, rotation);
        vfx.transform.rotation = _PlayerCharacter._PlayerActor.transform.rotation;
    }

    // end
}
