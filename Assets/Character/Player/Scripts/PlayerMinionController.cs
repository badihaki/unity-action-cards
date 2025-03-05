using Unity.VisualScripting;
using UnityEngine;

public class PlayerMinionController : CharacterGroupLeader
{
    [Header("Minion summoning"), SerializeField]
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
            Vector3 loc = _LeaderCharacter._Actor.transform.position;
            loc.x += Random.Range(minSummonDistance, maxSummonDistance);
            if (GameManagerMaster.GameMaster.Dice.RollD4() > 2)
                loc.x *= -1; // this moves it to left or the right randomly
            loc.z += Random.Range(minSummonDistance, maxSummonDistance); // may wanna move in front of the camera later
            loc.y += 1;
            // now use spherecast to determine if there's a collider there
            print($"location chosen is {loc}");
            Collider[] collisions = Physics.OverlapSphere(loc, 1);
			foreach (var collision in collisions)
			{
                print(collision.name);
			}
			if (collisions.Length == 0)
            {
				found = true;
                summonLocation = loc;
                break;
            }
		}

		return found;
    }

	private void SummonMinion(NPCSheetScriptableObj minionTemplate)
    {
        NonPlayerCharacter minion = ObjectPoolManager.GetObjectFromPool(GameManagerMaster.GameMaster.Resources.npcTemplate, summonLocation, Quaternion.identity, ObjectPoolManager.PoolFolder.Character, minionTemplate._CharacterName).GetComponent<NonPlayerCharacter>();
        minion.BuildAndInitialize(minionTemplate);
        CharacterGroupMember groupMember = minion.AddComponent<CharacterGroupMember>();
        groupMember.Initialize(this);
        minion.GetGroupedUp(groupMember);
        AddCharacterToGroup(minion);
    }
}
