using System.Collections.Generic;
using UnityEngine;

public class FireEffector : MonoBehaviour
{
	[SerializeField]
	private List<IFlammable> flammableEntitiesList;
	[SerializeField]
	private List<IFlammable> thoseImmuneToMyFlames;
	[SerializeField]
	private float fireDamageTimer;

	private void Initialize(IFlammable originator)
	{
		flammableEntitiesList = new List<IFlammable>();
		thoseImmuneToMyFlames = new List<IFlammable>
		{
			originator
		};
	}

	private void Start()
	{
		if (thoseImmuneToMyFlames == null || thoseImmuneToMyFlames.Count == 0)
			thoseImmuneToMyFlames = new List<IFlammable>();
	}

	private void Update()
	{
		if(flammableEntitiesList.Count > 0)
		{
			fireDamageTimer += Time.deltaTime;
			if (fireDamageTimer > 1.35f)
			{
				fireDamageTimer = 0;
			}
			for (int i = 0; i < flammableEntitiesList.Count; i++)
			{
				flammableEntitiesList[i].TakeFireDamage(1);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		other.TryGetComponent(out IFlammable flammableEntity);
		if (flammableEntity != null)
		{
			if (thoseImmuneToMyFlames.Contains(flammableEntity))
				return;
			flammableEntity.TakeFireDamage(1);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		other.TryGetComponent(out IFlammable flammableEntity);
		if (flammableEntity != null)
		{
			if (flammableEntitiesList.Contains(flammableEntity))
				flammableEntitiesList.Remove(flammableEntity);
		}
	}
}
