using UnityEngine;

public class ConstantVariables : MonoBehaviour
{
	[field: Header("Force values"), SerializeField, Tooltip("the force used to launch a character")]
	public float CharacterLaunchForce { get; private set; } = 25.85f;
	
	[field: SerializeField, Tooltip("X is for the pushback power (Z) and Y is for the launch force (Y)")]
	public Vector2 FarKnockBackForce { get; private set; } = new Vector2(15.3f, 23.4f);
}
