using UnityEngine;

public interface IAggressable
{
	public delegate void IsDamaged(int aggressionAmount, Character aggressor);
	event IsDamaged EntityIsDamaged;
}
