using UnityEngine;

public class NavNodeEditorHelper : MonoBehaviour
{
	[field: SerializeField] private Color _Color = Color.cyan;
	[field: SerializeField] private float _EditorSphereRadius = 10.0f;

	private void OnDrawGizmos()
	{
		Gizmos.color = _Color;
		Gizmos.DrawSphere(transform.position, _EditorSphereRadius);
	}
}
