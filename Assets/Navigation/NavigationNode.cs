using NUnit.Framework;
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
		if (Physics.SphereCast(transform.position, _NeighborDetectionRadius, transform.forward, out RaycastHit hitInfo))
		{
            print("hit");
            print(hitInfo);
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