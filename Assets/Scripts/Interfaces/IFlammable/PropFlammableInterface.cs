using System;
using UnityEngine;

public class PropFlammableInterface : MonoBehaviour, IFlammable
{
	[SerializeField]
	private EnvProp parentEnvProp;
	[SerializeField] 
	private float fireDmg;
	[SerializeField]
	private bool onFire;
	private BoxCollider trigger;
	[SerializeField]
	private GameObject fireEffect;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		parentEnvProp = GetComponentInParent<EnvProp>();
		trigger = GetComponent<BoxCollider>();
	}

	// Update is called once per frame
	void Update()
	{
		if (onFire)
			RunWhenOnFire();
	}

	public BoxCollider referenceCollider { get
		{
			return trigger;
		}
	}

	public float currentFireDmg { get => fireDmg; }

	public bool isOnFire { get => onFire; }

	public void SetOnFire()
	{
		fireEffect = ObjectPoolManager.GetObjectFromPool(GameManagerMaster.GameMaster.Resources.fireElementEffect, trigger.transform.position, Quaternion.identity, transform, ObjectPoolManager.PoolFolder.Particle);
		fireEffect.GetComponent<FireEffector>().Initialize(this);
		onFire = true;
	}

	public void RunWhenOnFire()
	{
		fireEffect.transform.position = trigger.transform.position;
	}

	public void TakeFireDamage(int damage)
	{
		print("takin fire damage");
		fireDmg += damage;
		if(fireDmg > 10)
		{
			fireDmg = 10;
		}
		if (fireDmg >= 7 && !onFire)
			SetOnFire();
		parentEnvProp._Health.TakeDamage((int)Math.Round(fireDmg));
	}
}
