using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCNavigator : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    private NPCAttackController _AttackController;
    private NPCMovementController _MovementController;

    // target
    [field: SerializeField, Header("Target info")] public Transform _NavTarget { get; private set; }
    [field: SerializeField] public float _MaxDistance { get; private set; }

    // node
    [field: SerializeField, Header("Nav Node")]
    public NavigationNode _CurrentNavNode { get; private set; }
    [field: SerializeField] private List<NavigationNode> _PriorNavNodes;
    [field: SerializeField] private bool navNodListIsReseting;
    private WaitForSeconds listWaitTimeForReset = new WaitForSeconds(9.386f);


    public void InitializeNavigator(NonPlayerCharacter npc)
    {
        _NPC = npc;
        navNodListIsReseting = false;
        _AttackController = _NPC._AttackController as NPCAttackController;
        _MovementController = _NPC.GetComponent<NPCMovementController>();
        _NPC._NPCActor._AggressionManager.OnBecomeAggressed += StopNodeListManagement;
    }

	private void OnEnable()
	{
		if(_NPC != null)
        {
			_NPC._NPCActor._AggressionManager.OnBecomeAggressed += StopNodeListManagement;
		}
	}

	private void OnDisable()
	{

		_NPC._NPCActor._AggressionManager.OnBecomeAggressed -= StopNodeListManagement;
	}


	#region Navigation Node
	
    #region Finding a Node
	public bool TryFindNewNavNode()
    {
        if (!_CurrentNavNode) // if we dont have a nav node, lets get one and pause for a minute
        {
            FindNewNavigationNode();
            return false;
        }
        FindNextNavigationNode();
        return true;
    }

    private void FindNewNavigationNode()
    {
        Collider[] nodes = Physics.OverlapSphere(transform.position, 20.0f, LayerMask.GetMask("Navigation"), QueryTriggerInteraction.UseGlobal);
        List<NavigationNode> navNodes = new List<NavigationNode>();
        foreach (var navNode in nodes)
        {
            NavigationNode NavNode = navNode.GetComponent<NavigationNode>();
            if(NavNode != null)
                navNodes.Add(NavNode);
        }
        _CurrentNavNode = GetNavNodeFromList(navNodes.ToArray());
		_MovementController.SetAgentDestination(_CurrentNavNode.transform.position);
	}

	private void FindNextNavigationNode()
    {
        NavigationNode[] nodes = _CurrentNavNode._Neighbors.ToArray();
        NavigationNode navNode = GetNavNodeFromList(nodes);
		_CurrentNavNode = navNode;
		_MovementController.SetAgentDestination(_CurrentNavNode.transform.position);
	}

    private NavigationNode GetNavNodeFromList(NavigationNode[] nodes)
    {
        NavigationNode navNode = null;
        navNode = nodes[Random.Range(0, nodes.Length - 1)];
		return navNode; // return a random node
    }
	#endregion

	private void StopNodeListManagement()
    {
        // stop managing the navigation node list
        StopCoroutine(ManageNavNodeRemoval());
        navNodListIsReseting = false;

        // get the target
        Character targetChar = _NPC._NPCActor._AggressionManager._LastAggressors.LastOrDefault();
		_NavTarget = targetChar._Actor.transform;
        _AttackController.TrySetNewTargetEnemy(targetChar); // set the enemy
        
        // clean up navigation nodes
        _PriorNavNodes.Clear();
        _CurrentNavNode = null;
		_MovementController.SetAgentDestination(null);
    }

	public void AddToPriorNodes()
    {
        _PriorNavNodes.Add(_CurrentNavNode);
        if (_PriorNavNodes.Count > 5) _PriorNavNodes.RemoveAt(0);
        if (!navNodListIsReseting)
        {
            navNodListIsReseting = true;
            StartCoroutine(ManageNavNodeRemoval());
        }
    }

    public IEnumerator ManageNavNodeRemoval()
    {
        while (navNodListIsReseting)
        {
			yield return listWaitTimeForReset;
            if(_PriorNavNodes.Count > 0)
            {
                _PriorNavNodes.RemoveAt(0);
				if (_PriorNavNodes.Count <= 0)
                {
                    navNodListIsReseting = false;
                }
                yield return null;
			}
            navNodListIsReseting = false;
        }
    }
    #endregion

    #region Target Acquisition and Management
	/// <summary>
	/// Set a new target using a basic Transform
	/// </summary>
	/// <param name="newTarget">Target transform that becomes the new _NavTarget</param>
	/// <param name="distance">Distance from the target</param>
	public void SetTarget(Transform newTarget, float distance = 1.0f)
	{
		_CurrentNavNode = null;
		if (newTarget != null)
		{
			_NavTarget = newTarget;
			_MovementController.SetAgentDestination(_NavTarget.transform.position, distance);
		}
		else
		{
			_NavTarget = null;
			_MovementController.SetAgentDestination(_NPC._Actor.transform.position);
		}
	}
    /// <summary>
    /// Set target using a character
    /// </summary>
    /// <param name="targetChar"></param>
    /// <param name="distance"></param>
    public void SetTarget(Character targetChar, float distance = 1.0f)
    {
		_CurrentNavNode = null;
        if (targetChar != null)
        {
            _NavTarget = targetChar._Actor.transform;
            targetChar._Actor.onDeath += RemoveTargetCharacter;
			_MovementController.SetAgentDestination(_NavTarget.transform.position, distance);
		}
		else
		{
			_NavTarget = null;
			_MovementController.SetAgentDestination(_NPC._Actor.transform.position);
		}
	}
    public void RemoveTarget()
    {
		_NavTarget = null;
        _MovementController.SetAgentDestination(_NPC._Actor.transform.position);
        print($"cleaning up:: removing the nav target");
	}
	public void RemoveTargetCharacter(Character targetChar)
    {
        print($"cleaning up:: making sure {targetChar.name} is not the target");
		if (_NavTarget == targetChar._Actor.transform)
            RemoveTarget();
        targetChar._Actor.onDeath -= RemoveTargetCharacter;
    }
	public void SetTargetDesiredDistance(float distance, float m_distance = 2.0f)
    {
        SetMaxAttackDistance(distance + 1.0f);
    }
    public void SetMaxAttackDistance(float m_distance = 2.0f) => _MaxDistance = m_distance;
	#endregion

    // end
}
