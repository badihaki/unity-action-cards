using System;
using System.Collections.Generic;
using UnityEngine;

public class FireEffector : MonoBehaviour
{
	[Serializable]
	private struct FlammableEntityStruct
	{
		public IFlammable flammableInterface;
		public Transform controller;

		public FlammableEntityStruct(IFlammable _flammableInterface)
		{
			flammableInterface = _flammableInterface;
			controller = flammableInterface.referenceCollider.transform.parent.transform;
		}
	}
	[field: SerializeField]
	private List<FlammableEntityStruct> flammableEntitiesList;
	[field: SerializeField]
	private List<FlammableEntityStruct> thoseImmuneToMyFlames;
	[SerializeField]
	private float fireDamageTimer;

	public void Initialize(IFlammable originator)
	{
		thoseImmuneToMyFlames = new List<FlammableEntityStruct>();
		FlammableEntityStruct originatorImmune = new FlammableEntityStruct(originator);
		BoxCollider col = GetComponent<BoxCollider>();
		Vector3 colliderSize = col.size * 1.1f;
		col.size = colliderSize;
	}

	private void Start()
	{
		flammableEntitiesList = new List<FlammableEntityStruct>();
		if (thoseImmuneToMyFlames == null || thoseImmuneToMyFlames.Count == 0)
		{
			thoseImmuneToMyFlames = new List<FlammableEntityStruct>();
		}
	}

	private void Update()
	{
		if(flammableEntitiesList.Count > 0)
		{
			fireDamageTimer += Time.deltaTime;
			if (fireDamageTimer > 1.35f)
			{
				fireDamageTimer = 0;
				for (int i = 0; i < flammableEntitiesList.Count; i++)
				{
					flammableEntitiesList[i].flammableInterface.TakeFireDamage(1);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		other.TryGetComponent(out IFlammable flammableEntity);
		if (flammableEntity != null)
		{
			print("that entity was flammable!!");
			FlammableEntityStruct flammable = new FlammableEntityStruct(flammableEntity);
			if (thoseImmuneToMyFlames.Count > 0)
			{
				for (int i = 0; i < thoseImmuneToMyFlames.Count; i++)
				{
					if (thoseImmuneToMyFlames[i].flammableInterface == flammableEntity)
					{
						return;
					}
				}
			}
			flammableEntity.TakeFireDamage(1);
			flammableEntitiesList.Add(flammable);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		other.TryGetComponent(out IFlammable flammableEntity);
		if (flammableEntity != null)
		{
			FlammableEntityStruct flammable = new FlammableEntityStruct(flammableEntity);
			if (flammableEntitiesList.Contains(flammable))
				flammableEntitiesList.Remove(flammable);
		}
	}
}
