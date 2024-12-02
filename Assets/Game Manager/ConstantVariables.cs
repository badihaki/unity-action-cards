using UnityEngine;

public class ConstantVariables : MonoBehaviour
{
	[field: Header("Force values"), SerializeField, Tooltip("the force used to launch a character")]
	public float CharacterLaunchForce { get; private set; } = 25.85f;
	[field: SerializeField]
	public Vector2 FarKnockBackForce { get; private set; } = new Vector2(5.3f, 3.4f);
}
