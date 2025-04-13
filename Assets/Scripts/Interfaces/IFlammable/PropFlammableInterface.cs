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

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		parentEnvProp = GetComponentInParent<EnvProp>();
	}

	// Update is called once per frame
	void Update()
	{
		if (onFire)
			RunWhenOnFire();
	}

	public Collider referenceCollider { get
		{
			return parentEnvProp._Collider;
		}
	}

	public float currentFireDmg { get => fireDmg; }

	public bool isOnFire { get => onFire; }

	public void RunWhenOnFire()
	{
		print($"ahh {parentEnvProp.name} is on fire!!!");
	}

	public void TakeFireDamage(int damage)
	{
		fireDmg += damage;
	}
}
