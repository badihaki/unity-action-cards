using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAggressionManager : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    private NPCAttackController _AttackController;
	[field: SerializeField, Header("Aggression State")]
    public int _Aggression { get; private set; }
    [field: SerializeField]
    public List<Character> _LastAggressors {  get; private set; }
    [field: SerializeField]
    public bool isAggressive { get; private set; }

    // waits
    private WaitForSeconds slowWait = new WaitForSeconds(1.35f);
    private WaitForSeconds fastWait = new WaitForSeconds(0.35f);

    // events
    public delegate void OnAggressed();
    public event OnAggressed OnBecomeAggressed;
    public delegate void OnSeeAlly(Character character);
    public event OnSeeAlly FoundFriend;

	public void Initialize(NonPlayerCharacter npc)
    {
        _NPC = npc;
        _AttackController = _NPC._AttackController as NPCAttackController;
		_Aggression = 0;
        _LastAggressors = new List<Character>();
    }

    public void AddAggression(int aggression, Character aggressor)
    {
        _Aggression += aggression;
        if (_Aggression > 100)
            _Aggression = 100;
        
        if(!_LastAggressors.Contains(aggressor) && _LastAggressors.Count < 4)
        {
            if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
                print(">>>>>>> adding aggressor <<");
            _LastAggressors.Add(aggressor);
            aggressor._Actor.onDeath += _NPC._NavigationController.RemoveTargetCharacter;
            aggressor._Actor.onDeath += RemoveAggressor;
		}

        if (_Aggression >= 50 && !isAggressive)
        {
            isAggressive = true;
			_NPC._NPCActor.animationController.SetBool("aggressive", true);
            //print("aggressed in animator <<<<<<<<<<<<<<");
			OnBecomeAggressed();
			StartCoroutine(SlowlyLowerAggression());
        }
        if (_Aggression > 100) _Aggression = 100;
    }

    private void RemoveAggressor(Character aggressor)
    {
		_LastAggressors?.Remove(aggressor);
        _AttackController.RemoveActiveTarget(aggressor);
        if (_LastAggressors.Count > 0)
        {
            Character nextEnemy;
			if (_LastAggressors.Count == 1)
                nextEnemy = _LastAggressors[0];
            else
            {
                int enemyIndex = Random.Range(0, _LastAggressors.Count - 1);
                nextEnemy = _LastAggressors[enemyIndex];
            }
            _AttackController.TrySetNewTargetEnemy(nextEnemy);
        }
        else
        {
            StopAllCoroutines();
            _Aggression = 0;
            isAggressive = false;
			_NPC._NPCActor.animationController.SetBool("aggressive", false);
		}
		aggressor._Actor.onDeath -= _NPC._NavigationController.RemoveTargetCharacter;
		aggressor._Actor.onDeath -= RemoveAggressor;
	}

    private IEnumerator SlowlyLowerAggression()
    {
        while(_Aggression >= 35)
        {
            yield return slowWait;
            _Aggression -= 1;
            int targetIndex = _Aggression;
			foreach (var enemy in _LastAggressors)
			{
			    if (_Aggression < 50 && _NPC._EyeSight.CanSeeTarget(enemy._Actor.transform))
                {
                    _Aggression = 100;
                    _AttackController.TrySetNewTargetEnemy(enemy);                    
                    break;
                }				
			}
            yield return null;
        }
        StartCoroutine(QuicklyLowerAggression());
        StopCoroutine(SlowlyLowerAggression());
    }
    private IEnumerator QuicklyLowerAggression()
    {
        while (isAggressive)
        {
            yield return fastWait;
            _Aggression -= 1;
            if (GameManagerMaster.GameMaster.GMSettings.logExraPlayerData)
                print("quickly lose aggression");
			if (_Aggression == 0)
            {
				// lost all aggression here
				foreach (Character aggressor in _LastAggressors)
				{
                    RemoveAggressor(aggressor);
				}
                if (_LastAggressors.Count > 0)
                    _LastAggressors.Clear();
                isAggressive = false;
                _NPC._NPCActor.animationController.SetBool("aggressive", false);
                _NPC._StateMachine.ChangeState(_NPC._StateMachine._StateLibrary._IdleState);
				StopCoroutine(QuicklyLowerAggression());
			}
			else
                yield return null;
        }
    }

	public bool CheckIfValidEnemy(Character character)
	{
		Actor charActor = character._Actor;
        if (_NPC.isGroupedUp)
        {
            if (character.TryGetComponent(out CharacterGroupMember groupMember))
            {
                // got member
                // then we check to see if the group leader is our leader. this should work for the player on both sides as well
                if (groupMember.CheckIfPartOfMyGroup(groupMember))
                {
                    if (FoundFriend != null)
                        FoundFriend.Invoke(character);
                    return false;
                }
            }
            else
            {
				// no member
				if (!_NPC._TypesManager.SharesTypeWith(character as NonPlayerCharacter))
				{
					return true; // doesnt share type, is enemy
				}
                return false; // shares type, not enemy
			}
        }
		if (character is PlayerCharacter)
		{
            if (_NPC._NPCharacterSheet.isAlwaysAggressive)
                return true; // character is a player and I'm always an aggressive lil bish
		}
		else
		{
			if (!_NPC._TypesManager.SharesTypeWith(character as NonPlayerCharacter))
			{
                return true;
			}
		}
        return false;
	}
	//
}
