using System;
using UnityEngine;

[Serializable]
public class Damage
{
	[field: SerializeField]
	public int damageAmount {  get; private set; }
	//public int healAmount { get; private set; }
	
	[field: SerializeField]
	public float poiseDamageAmount { get; private set; }

	[field: SerializeField]
	public responsesToDamage intendedResponse { get; private set; }
	[field: SerializeField]
	public float damageForce { get; private set; }
	[field: SerializeField]
	public Transform damageSource { get; private set; }
	[field: SerializeField]
	public Character damageCreatorCharacter { get; private set; }

	[field: SerializeField]
	public DamageOptions dmgOptions { get; private set; }

	public Damage(int dmg, float force, responsesToDamage response, Transform source, Character creator)
	{
		damageAmount = dmg;
		poiseDamageAmount = CalculatePoise(dmg);
		damageForce = force;
		Mathf.Clamp(force, 0, 1);
		intendedResponse = response;
		damageSource = source;
		damageCreatorCharacter = creator;

		dmgOptions = new DamageOptions(0, 0, 0, 0, Vector2.zero);
	}

	private float CalculatePoise(int dmg)
	{
		float calculatedPoise = dmg * UnityEngine.Random.Range(15, 25);
		return calculatedPoise;
	}

	public void SetOptions(DamageOptions dmgOptions)
	{
		SetFireDamage(dmgOptions.fireDmg);
		SetWaterDamage(dmgOptions.waterDmg);
		SetPlasmaDamage(dmgOptions.plasmaDmg);
		SetGravityDamage(dmgOptions.gravityDmg, dmgOptions.gravityInflunce);
	}

	public void SetFireDamage(float value)
	{
		DamageOptions copy = dmgOptions;
		copy.fireDmg = value;
		dmgOptions = copy;
	}
	public void SetWaterDamage(float value)
	{
		DamageOptions copy = dmgOptions;
		copy.waterDmg = value;
		dmgOptions = copy;
	}
	public void SetPlasmaDamage(float value)
	{
		DamageOptions copy = dmgOptions;
		copy.plasmaDmg = value;
		dmgOptions = copy;
	}
	public void SetGravityDamage(float value, Vector2 influence)
	{
		DamageOptions copy = dmgOptions;
		copy.gravityDmg = value;
		dmgOptions = copy;
	}
}

[Serializable]
public struct DamageOptions
{

	[field: SerializeField]
	public float fireDmg;
	[field: SerializeField]
	public float waterDmg;
	[field: SerializeField]
	public float plasmaDmg;
	[field: SerializeField]
	public float gravityDmg;
	[field: SerializeField]
	public Vector2 gravityInflunce;
	public DamageOptions(float fire, float water, float plasma, float gravity, Vector2 influence)
	{
		fireDmg = fire;
		waterDmg = water;
		plasmaDmg = plasma;
		gravityDmg = gravity;
		gravityInflunce = influence;
	}
}

public enum responsesToDamage
{
	hit, // this can do air hits and ground hits
	stagger,
	knockBack,
	launch,
}