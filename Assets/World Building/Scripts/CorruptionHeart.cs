using UnityEngine;

public class CorruptionHeart : MonoBehaviour, IDamageable
{
    public Health _Health { get; private set; }

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
}
