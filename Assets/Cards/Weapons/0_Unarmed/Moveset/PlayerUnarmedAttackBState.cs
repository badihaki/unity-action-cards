using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Attack B", menuName = "Create Attacks/00_Unarmed/Unarmed Attack B")]
public class PlayerUnarmedAttackBState : PlayerAttackSuperState
{
    public PlayerUnarmedAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _PlayerCharacter._AttackController.SetAttackParameters(2, 1.025f, 1.35f);
        ShowOrHideWeapon(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        ShowOrHideWeapon(false);
    }

    public override void CheckStateTransitions()
    {
        if (canCombo && attackInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._AttackC);

        base.CheckStateTransitions();
    }
    public override void TriggerVisualEffect()
    {
        base.TriggerVisualEffect();
        Vector3 position = new Vector3(_PlayerCharacter.transform.position.x, _PlayerCharacter.transform.position.y + 0.75f, _PlayerCharacter.transform.position.z);
        Quaternion rotation = Quaternion.Euler(_PlayerCharacter._PlayerActor.transform.forward);

        GameObject vfx = Instantiate(_PlayerCharacter._AttackController._CurrentWeapon._WeaponAttackFX, position, rotation);
        vfx.transform.rotation = _PlayerCharacter._PlayerActor.transform.rotation;
    }

    // end
}
