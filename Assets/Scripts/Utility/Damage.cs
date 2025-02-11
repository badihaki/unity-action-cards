using UnityEngine;

public class Damage
{
	public int damageAmount {  get; private set; }
	//public int healAmount { get; private set; }
	public float poiseDamageAmount { get; private set; }

	public responsesToDamage intendedResponse { get; private set; }
	public float damageForce { get; private set; }
	public Transform damageSource { get; private set; }

	public Damage(int dmg, float force, responsesToDamage response, Transform source)
	{
		damageAmount = dmg;
		poiseDamageAmount = CalculatePoise(dmg);
		damageForce = force;
		intendedResponse = response;
		damageSource = source;
	}

	private float CalculatePoise(int dmg)
	{
		float calculatedPoise = dmg * Random.Range(15, 25);
		return calculatedPoise;
	}
}

public enum responsesToDamage
{
	hit, // this can do air hits and ground hits
	stagger,
	knockBack,
	launch,
}