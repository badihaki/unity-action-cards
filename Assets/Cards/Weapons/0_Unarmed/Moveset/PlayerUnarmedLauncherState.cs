using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Launcher", menuName = "Create Attacks/00_Unarmed/Unarmed Launcher")]
public class PlayerUnarmedLauncherState : PlayerLauncherAttackSuperState
{
    public PlayerUnarmedLauncherState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _PlayerCharacter._AttackController.SetAttackParameters(1, 0.78f, 110.75f);
    }


    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (canCombo && jumpInput) _StateMachine.ChangeState(_StateMachine._JumpState);
    }
}
