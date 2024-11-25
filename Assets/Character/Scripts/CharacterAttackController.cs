using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
	private Character _Character;

	[field: Header("Attack Stats"), SerializeField]
	public int _Damage { get; protected set; }
	[field: SerializeField] public float _KnockbackForce { get; protected set; }
	[field: SerializeField] public float _LaunchForce { get; protected set; }

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public virtual void Initialize(Character character)
	{
		_Character = character;
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
