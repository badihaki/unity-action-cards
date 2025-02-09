using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCNavigator : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    [field: SerializeField, Header("Target info")] public Transform _Target { get; private set; }
    [field: SerializeField] public float _MaxDistance { get; private set; }

    [field: SerializeField, Header("Nav Node")]
    public NavigationNode _CurrentNavNode { get; private set; }
    [field: SerializeField] private List<NavigationNode> _PriorNavNodes;
    [field: SerializeField] private bool listReseting;
    private WaitForSeconds listWaitTime = new WaitForSeconds(9.386f);
    private float resetTimer;
    private NPCAttackController _AttackController;

    public void InitializeNavigator(NonPlayerCharacter npc)
    {
        _NPC = npc;
        listReseting = false;
        _AttackController = _NPC._AttackController as NPCAttackController;
        _NPC._NPCActor._AggressionManager.IsAggressed += BecomeAggressed;
    }

	private void OnEnable()
	{
		if(_NPC != null)
        {
			_NPC._NPCActor._AggressionManager.IsAggressed += BecomeAggressed;
		}
	}

	private void OnDisable()
	{

		_NPC._NPCActor._AggressionManager.IsAggressed -= BecomeAggressed;
	}

	private void BecomeAggressed()
    {
        // stop managing the navigation node list
        StopCoroutine(ManageNavNodeList());
        listReseting = false;

        // get the target
        _Target = _NPC._NPCActor._AggressionManager._LastAggressors.LastOrDefault();
        _AttackController.SetNewTargetEnemy(_Target); // set the enemy
        
        // clean up navigation nodes
        _PriorNavNodes.Clear();
        _CurrentNavNode = null;
        _NPC._MoveController.SetAgentDestination(null);
    }

    public bool TryFindNewPatrol()
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
        Collider[] nodes = Physics.OverlapSphere(transform.position, 10.5f, LayerMask.GetMask("Navigation"), QueryTriggerInteraction.UseGlobal);
        List<NavigationNode> navNodes = new List<NavigationNode>();
        //NavigationNode node = nodes[Random.Range(0, nodes.Length-1)].GetComponent<NavigationNode>();
        foreach (var navNode in nodes)
        {
            NavigationNode NavNode = navNode.GetComponent<NavigationNode>();
            if(NavNode != null)
                navNodes.Add(NavNode);
        }
        _CurrentNavNode = GetNewNavNode(navNodes.ToArray());
        _NPC._MoveController.SetAgentDestination(_CurrentNavNode.transform.position, 3.0f);
	}

	private void FindNextNavigationNode()
    {
        NavigationNode[] nodes = _CurrentNavNode._Neighbors.ToArray();
        NavigationNode navNode = GetNewNavNode(nodes);
        _CurrentNavNode = navNode;
		_NPC._MoveController.SetAgentDestination(_CurrentNavNode.transform.position, 3.2f);
	}
    private NavigationNode GetNewNavNode(NavigationNode[] nodes)
    {
        if (GameManagerMaster.GameMaster.GMSettings.logNPCNavData)
        {
            foreach (var node in nodes)
            {
                print($"got navnode {node.name}");
            }
        }
        NavigationNode navNode = null;

        navNode = nodes[UnityEngine.Random.Range(0, nodes.Length - 1)];

		return navNode; // return a random node
            
    }

	public void SetTarget(Transform newTarget) => _Target = newTarget;
    public void SetTargetDesiredDistance(float distance, float m_distance = 2.0f)
    {
        SetMaxAttackDistance(distance + 1.0f);
    }
    public void SetMaxAttackDistance(float m_distance = 2.0f) => _MaxDistance = m_distance;

    public void AddToPriorNodes()
    {
        _PriorNavNodes.Add(_CurrentNavNode);
        if (_PriorNavNodes.Count > 5) _PriorNavNodes.RemoveAt(0);
        if (!listReseting)
        {
            listReseting = true;
            StartCoroutine(ManageNavNodeList());
        }
    }

    public IEnumerator ManageNavNodeList()
    {
        while (listReseting)
        {
            resetTimer = Time.time;
			yield return listWaitTime;
            if(_PriorNavNodes.Count > 0)
            {
                _PriorNavNodes.RemoveAt(0);
				if (_PriorNavNodes.Count <= 0)
                {
                    listReseting = false;
                }
                yield return null;
			}
            listReseting = false;
        }
    }
}
