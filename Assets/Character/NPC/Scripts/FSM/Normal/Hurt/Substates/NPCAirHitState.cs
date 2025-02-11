using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Hurt/Air Hit", fileName = "Air Hit")]
public class NPCAirHitState : NPCHurtSuperState
{
	public NPCAirHitState()
	{
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		_NPC._MoveController.ApplyGravity(0.1f);
		_NPC._MoveController.ApplyExternalForces();
	}
}
