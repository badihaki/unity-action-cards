using UnityEngine;

public class CorruptionHeart : MonoBehaviour, IDamageable, IDestroyable
{
    public Health _Health { get; private set; }
	public delegate void CorruptionHeartDestroyed(CorruptionHeart heart);
	public CorruptionHeartDestroyed OnCorruptionHeartDestroyed;
	[field: SerializeField]
	public GameObject _CorruptionPortal { get; private set; }

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        _Health = GetComponent<Health>();
        _Health.InitiateHealth(25);
    }

    // Update is called once per frame
    void Update()
    {

	}

	public void Damage(Damage damageObj)
	{
		_Health.TakeDamage(damageObj.damageAmount);
	}

	public Transform GetControllingEntity()
	{
		return transform;
	}

	public Transform GetDamagedEntity()
	{
		return transform;
	}

	public void DestroyEntity()
	{
		OnCorruptionHeartDestroyed?.Invoke(this);
		Instantiate(_CorruptionPortal, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
