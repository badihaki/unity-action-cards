using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavigator : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    [field: SerializeField, Header("Target info")] public Transform _Target { get; private set; }
    [field: SerializeField] public float _MaxDistance { get; private set; }

    [field: SerializeField, Header("Nav Node")]
    private NavigationNode _CurrentNavNode;
    [field: SerializeField] private List<NavigationNode> _PriorNavNodes;

    public void InitializeNavigator(NonPlayerCharacter npc)
    {
        _NPC = npc;
    }

    public bool TryFindNewPatrol()
    {
        if (!_CurrentNavNode) // if we dont have a nav node, lets get one and pause for a minute
        {
            FindNewNavigationNode();
            return false;
        }
        return true;
    }

	private void FindNewNavigationNode()
	{
        NavigationNode[] nodes = GameObject.FindObjectsByType<NavigationNode>(FindObjectsSortMode.None);
        NavigationNode node = null;
        foreach (var navNode in nodes)
        {
            if (!node)
			{
				node = navNode;
				print($">>>>> starting node is {node}");
            }
            else
            {
                float distanceFromSavedNode = Vector3.Distance(_NPC._NPCActor.transform.position, node.transform.position);
                float distanceFromNavNode = Vector3.Distance(_NPC._NPCActor.transform.position, navNode.transform.position);
				if (distanceFromSavedNode > distanceFromNavNode) 
                {
					node = navNode;
                    print($">>>>> changing saved node to {node}");
                }
            }
        }
        _CurrentNavNode = node;
    }

	public void SetTarget(Transform newTarget) => _Target = newTarget;
    public void SetTargetDesiredDistance(float distance, float m_distance = 2.0f)
    {
        SetMaxAttackDistance(distance + 1.0f);
    }
    public void SetMaxAttackDistance(float m_distance = 2.0f) => _MaxDistance = m_distance;
}
