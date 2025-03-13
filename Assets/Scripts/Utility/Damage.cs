using System;
using UnityEngine;

public class Damage
{
	public int damageAmount {  get; private set; }
	//public int healAmount { get; private set; }
	public float poiseDamageAmount { get; private set; }

	public responsesToDamage intendedResponse { get; private set; }
	public float damageForce { get; private set; }
	public Transform damageSource { get; private set; }
	public Character damageCreatorCharacter { get; private set; }

	private DamageOptions dmgOptions;

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

public struct DamageOptions
{
	public float fireDmg;
	public float waterDmg;
	public float plasmaDmg;
	public float gravityDmg;
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