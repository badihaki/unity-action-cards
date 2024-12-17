using UnityEngine;
using System.Collections.Generic;

public class NavigationNode : MonoBehaviour
{
    [field:SerializeField] private Color _Color =  Color.cyan;
    [field: SerializeField] private float _EditorSphereRadius = 1.0f;
    [field: SerializeField] private float _NeighborDetectionRadius = 10.0f;
	[field: SerializeField] public List<NavigationNode> _Neighbors;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		//if (Physics.SphereCast(transform.position, _NeighborDetectionRadius, transform.forward, out RaycastHit hitInfo, 0.0f, LayerMask.GetMask("Navigation"), QueryTriggerInteraction.UseGlobal))
		FindNeightbors();
		Destroy(gameObject.GetComponent<NavNodeEditorHelper>());
		//Destroy(gameObject.GetComponent<SphereCollider>());
	}

	public void FindNeightbors()
	{
		Collider[] colliders = (Physics.OverlapSphere(transform.position, _NeighborDetectionRadius, LayerMask.GetMask("Navigation"), QueryTriggerInteraction.UseGlobal));
		foreach (var item in colliders)
		{
			NavigationNode navNode = item.GetComponent<NavigationNode>();
			if (navNode && navNode != this) _Neighbors.Add(navNode);
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = _Color;
        Gizmos.DrawSphere(transform.position, _EditorSphereRadius);
	}
}
