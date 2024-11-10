using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NavigationNode : MonoBehaviour
{
    [field:SerializeField] private Color _Color =  Color.cyan;
    [field: SerializeField] private float _Radius = 1.0f;
    [field: SerializeField] public List<NavigationNode> _Neighbors;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = _Color;
        Gizmos.DrawSphere(transform.position, _Radius);
	}
}
