using UnityEngine;

public interface IAggressable
{
	public delegate void IsDamaged(int aggressionAmount, Transform aggressor);
	event IsDamaged EntityIsDamaged;
}
