using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Movement/Stalk", fileName = "Stalking Move")]
public class NPCStalkngMoveState : NPCMoveState
{
	[field: SerializeField]
	private float lookForTargetsTimer;
	private NPCAttackController attackController;

	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);
		lookForTargetsTimer = GameManagerMaster.GameMaster.Dice.RollD4();
		attackController = _NPC._AttackController as NPCAttackController;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		ManageLookTimer();
	}
	public override void CheckStateTransitions()
	{
		if(attackController._ActiveTarget != null)
		{
			_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleAggressiveState);
		}

		base.CheckStateTransitions();
	}

	private void ManageLookTimer()
	{
		if (lookForTargetsTimer > 0)
		{
			lookForTargetsTimer -= Time.deltaTime;
			if (lookForTargetsTimer < 0)
			{
				if (GameManagerMaster.GameMaster.GMSettings.logNPCUtilData)
					_StateMachine.LogFromState("Looking for an enemy");
				LookForTargets();
				lookForTargetsTimer = GameManagerMaster.GameMaster.Dice.RollRandomDice(0.2f, 2.0f);
			}
		}
	}
	private void LookForTargets()
	{
		if (_NPC._EyeSight._NoticeableEntitiesInView.Count > 0)
		{
			foreach (var entity in _NPC._EyeSight._NoticeableEntitiesInView)
			{
				Character possibleCharacter = entity.GetComponentInParent<Character>();
				if (possibleCharacter != null)
					CheckIfValidEnemy(possibleCharacter);
			}
		}
	}

	protected void CheckIfValidEnemy(Character character)
	{
		Actor charActor = character._Actor;
		if (character is PlayerCharacter)
		{
			_NPC._NPCActor._AggressionManager.AddAggression(100, charActor.transform);
		}
		else
		{
			if (!SharesTypeWith(character as NonPlayerCharacter))
			{
				_NPC._NPCActor._AggressionManager.AddAggression(100, charActor.transform);
			}
		}
	}

	private bool SharesTypeWith(NonPlayerCharacter character)
	{
		foreach (var type in character._NPCharacterSheet.CharacterTypes)
		{
			if (_NPC._NPCharacterSheet.CharacterTypes.Contains(type))
				return true;
		}
		return false;
	}
}
