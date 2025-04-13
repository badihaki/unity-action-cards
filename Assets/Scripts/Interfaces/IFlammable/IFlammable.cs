using UnityEngine;

public interface IFlammable
{
	public void TakeFireDamage(int damage);
	public Collider referenceCollider { get; }
	public float currentFireDmg { get; }
	public bool isOnFire { get; }
}
