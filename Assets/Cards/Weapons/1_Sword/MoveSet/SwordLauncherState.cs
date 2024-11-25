using UnityEngine;

[CreateAssetMenu(fileName = "Sword Launcher", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Launcher")]
public class SwordLauncherState : PlayerLauncherAttackSuperState
{
	public SwordLauncherState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	public override void EnterState()
	{
		base.EnterState();
		_AttackController.SetAttackParameters(0.78f, 50.75f, 1);
	}
}
