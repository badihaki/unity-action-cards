using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Air Special", menuName = "Create Attacks/00_Unarmed/Unarmed Air Special")]
public class PlayerUnarmedAirSpecialState : PlayerAirCombatSuperState
{
    public PlayerUnarmedAirSpecialState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    private bool continued = false;

    public override void EnterState()
    {
        base.EnterState();

        continued = false;
        _PlayerCharacter._AnimationController.SetBool("continue", false);
        _PlayerCharacter._AnimationController.SetBool("continue2", false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!continued && _PlayerCharacter._CheckGrounded.IsGrounded())
        {
            continued = true;
            _PlayerCharacter._AnimationController.SetBool("continue", true);
            _PlayerCharacter._AnimationController.SetBool("continue2", true);
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
