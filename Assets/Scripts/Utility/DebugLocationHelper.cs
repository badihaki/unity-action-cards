using UnityEngine;

public class DebugLocationHelper : MonoBehaviour
{
    [field: SerializeField]
    private float radius = 0.25f;

	private void Start()
	{
		//SphereCollider newSphere = gameObject.AddComponent<SphereCollider>();
		//newSphere.isTrigger	= true;
		//newSphere.radius = radius;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, radius);
	}

	// end
}
