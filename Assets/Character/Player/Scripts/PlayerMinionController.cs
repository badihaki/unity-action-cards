using Unity.VisualScripting;
using UnityEngine;

public class PlayerMinionController : CharacterGroupLeader
{
    [Header("Minion summoning (Player Controls)"), SerializeField]
    private float minSummonDistance; // may have to put this in the summon card??? each card may have to define it's own distance.
    [SerializeField]
    private float maxSummonDistance; // may have to put this in the summon card??? each card may have to define it's own distance.
    [SerializeField]
    private Vector3 summonLocation;

	public bool TrySummonMinion(NPCSheetScriptableObj minionTemplate)
    {
        if (FindSpaceToSummon())
        {
            SummonMinion(minionTemplate);
            return true;
        }
        return false;
    }
    private bool FindSpaceToSummon()
    {
        bool found = false;
		for (int i = 0; i < 1000; i++)
		{
            // use search range to determine if there's a place to spawn
			Vector3 newSummonPos = CalculateSummonPosition();
            
			print($"summoning originates from {_LeaderCharacter._Actor.transform.position.ToString()}. Summoning position is {newSummonPos.ToString()}.");
            // now use spherecast to determine if there's a collider there
            Collider[] collisions = Physics.OverlapSphere(newSummonPos, 1);
			if (collisions.Length == 0)
			{
				found = true;
				summonLocation = newSummonPos;
				break;
			}
		}

		return found;
    }

	private Vector3 CalculateSummonPosition()
	{
		float summonX = Random.Range(minSummonDistance, maxSummonDistance);
		float summonZ = Random.Range(minSummonDistance, maxSummonDistance);
		Vector3 newSummonPos = Vector3.zero;
		if (GameManagerMaster.GameMaster.Dice.RollD4() > 2)
		{
            summonX *= -1;
		}
		//print($"summon position is {summonX}(X), {summonZ}(Z)");


		Vector3 fromSummonPos = _LeaderCharacter._Actor.transform.position;
		newSummonPos.x = fromSummonPos.x + summonX;
		newSummonPos.z = fromSummonPos.z + summonZ;
		newSummonPos.y = fromSummonPos.y + 1.25f;
		return newSummonPos;
	}

	private void SummonMinion(NPCSheetScriptableObj minionTemplate)
    {
        NonPlayerCharacter minion = ObjectPoolManager.GetObjectFromPool(GameManagerMaster.GameMaster.Resources.npcTemplate, summonLocation, Quaternion.identity, ObjectPoolManager.PoolFolder.Character, minionTemplate._CharacterName).GetComponent<NonPlayerCharacter>();
        minion.BuildAndInitialize(minionTemplate);
        CharacterGroupMember groupMember = minion.AddComponent<CharacterGroupMember>();
        groupMember.Initialize(this);
        minion.GetGroupedUp(groupMember);
        AddCharacterToGroup(minion);
        if (!_Character.isGroupedUp)
            _Character.GetGroupedUp(this);
    }
}
