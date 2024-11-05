using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Attack C", menuName = "Create Attacks/00_Unarmed/Unarmed Attack C")]
public class PlayerUnarmedAttackCState : PlayerAttackSuperState
{
    public PlayerUnarmedAttackCState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _PlayerCharacter._AttackController.SetAttackParameters(2, 0.75f, 1);
        ShowOrHideWeapon(true);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (canCombo && specialInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._FinisherA);
        if (canCombo && jumpInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._LauncherAttack);
    }

    public override void ExitState()
    {
        base.ExitState();
        ShowOrHideWeapon(false);
    }
    public override void TriggerVisualEffect()
    {
        base.TriggerVisualEffect();
        Vector3 position = new Vector3(_PlayerCharacter.transform.position.x, _PlayerCharacter.transform.position.y + 0.75f, _PlayerCharacter.transform.position.z);
        Quaternion rotation = Quaternion.Euler(_PlayerCharacter._PlayerActor.transform.forward);

        GameObject vfx = Instantiate(_PlayerCharacter._WeaponController._CurrentWeapon._WeaponAttackFX, position, rotation);
        vfx.transform.rotation = _PlayerCharacter._PlayerActor.transform.rotation;
    }
}
