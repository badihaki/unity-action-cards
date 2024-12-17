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
        StopCoroutine(ManageNavNodeList());
        _Target = _NPC._NPCActor._AggressionManager._LastAggressors.LastOrDefault();
        _AttackController.SetNewTarget(_Target);
        _PriorNavNodes.Clear();
        _CurrentNavNode = null;
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
        //NavigationNode[] nodes = GameObject.FindObjectsByType<NavigationNode>(FindObjectsSortMode.None);
		Collider[] nodes = (Physics.OverlapSphere(transform.position, 10.5f, LayerMask.GetMask("Navigation"), QueryTriggerInteraction.UseGlobal));
		NavigationNode node = null;
        foreach (var navNode in nodes)
        {
            NavigationNode NavNode = navNode.GetComponent<NavigationNode>();
			if (!node)
			{
				//node = navNode;
				node = NavNode;
			}
            else
            {
                float distanceFromSavedNode = Vector3.Distance(_NPC._NPCActor.transform.position, node.transform.position);
                float distanceFromNavNode = Vector3.Distance(_NPC._NPCActor.transform.position, navNode.transform.position);
				if (distanceFromSavedNode > distanceFromNavNode) 
                {
					//node = navNode;
					node = NavNode;
                }
            }
        }
        _CurrentNavNode = node;
    }

    private void FindNextNavigationNode()
    {
        NavigationNode[] nodes = _CurrentNavNode._Neighbors.ToArray();
        NavigationNode navNode = GetNewNavNode(nodes);        
        _CurrentNavNode = navNode;
    }
    private NavigationNode GetNewNavNode(NavigationNode[] nodes)
    {
        NavigationNode navNode = null;
        foreach (var node in nodes)
        {
            if (!_PriorNavNodes.Contains(node))
            {
                navNode = node; // just get the first node we haven't been to yet
                break;
            }
        }
        if(navNode != null)
            return navNode;
        else
        {

            return nodes[UnityEngine.Random.Range(0, nodes.Length - 1)]; // return a random node
        }
            
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
        while (_PriorNavNodes.Count > 0)
        {
            resetTimer = Time.time;
			yield return listWaitTime;
            //print($"removing {_PriorNavNodes[0]} from > LIST < of prior nodes");
            //print($" Elapsed Time == {Time.time - resetTimer}");
            _PriorNavNodes.RemoveAt(0);
            yield return null;
        }
        listReseting = false;
    }
}
