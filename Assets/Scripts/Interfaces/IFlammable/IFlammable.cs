using UnityEngine;

public interface IFlammable
{
	public void SetOnFire();
	public void TakeFireDamage(float damage);
	Transform GetControllingEntity();
	public BoxCollider referenceCollider { get; }
	public float currentFireDmg { get; }
	public bool isOnFire { get; }
}
