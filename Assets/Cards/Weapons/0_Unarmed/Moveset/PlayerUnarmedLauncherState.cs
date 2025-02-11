using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Launcher", menuName = "Characters/Player/Create Attacks/00_Unarmed/Unarmed Launcher")]
public class PlayerUnarmedLauncherState : PlayerLauncherAttackSuperState
{
    public PlayerUnarmedLauncherState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //_AttackController.SetAttackParameters(false, true);
        int damage = _PlayerCharacter._WeaponController._CurrentWeapon._Dmg;
        _AttackController.SetAttackParameters(responsesToDamage.launch);
    }
}
