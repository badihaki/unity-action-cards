using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
	public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
	public enum PoolFolder
	{
		None,
		Character,
		Projectile,
		Particle,
	}
	public static PoolFolder IntendedPoolFolder;

	public static GameObject SpawnObject(GameObject objToSpwn, Vector3 spwnPos, Quaternion spwnRot, PoolFolder poolFolder = PoolFolder.None)
	{
		PooledObjectInfo pool = ObjectPools.Find(pool => pool.LookupString == objToSpwn.name);
		// if the pool doesnt exist
		if (pool == null)
		{
			// setup a new pool
			pool = new PooledObjectInfo() { LookupString = objToSpwn.name };
			ObjectPools.Add(pool);
		}

		// check if there are any inactive objects
		GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

		if (spawnableObject == null) // we make a new one
		{
			GameObject parentObj = SetParentObject(poolFolder).gameObject;
			spawnableObject = Instantiate(objToSpwn, spwnPos, spwnRot);
			if (parentObj != null)
			{
				spawnableObject.transform.SetParent(parentObj.transform); 
			}
		}
		else // we reset its pos/rot, remove it from the list, set active to true
		{
			spawnableObject.transform.position = spwnPos;
			spawnableObject.transform.rotation = spwnRot;
			pool.InactiveObjects.Remove(spawnableObject);
			spawnableObject.SetActive(true);
		}

		return spawnableObject;
	}

	public static void ReturnObjectToPool(GameObject obj)
	{
		string gameObjName = obj.name.Replace("(Clone)", string.Empty);
		PooledObjectInfo pool = ObjectPools.Find(pool => pool.LookupString == gameObjName);

		if(pool == null)
		{
			Debug.LogError($"Trying to return an object to a pool that doesn't exist?! Object is {obj.ToString()}");
			return;
		}
		obj.SetActive(false);
		pool.InactiveObjects.Add(obj);
	}

	private static Transform SetParentObject(PoolFolder parentFolder)
	{
		switch (parentFolder)
		{
			case PoolFolder.Character:
				return GameManagerMaster.GameMaster.GeneralConstantVariables.GetCharactersFolder();
			case PoolFolder.Projectile:
				return GameManagerMaster.GameMaster.GeneralConstantVariables.GetProjectilesFolder();
			case PoolFolder.Particle:
				return GameManagerMaster.GameMaster.GeneralConstantVariables.GetParticlesFolder();
			case PoolFolder.None:
				return null;
			default:
				return null;
		}
	}
	
	// end of this class
}


public class PooledObjectInfo
{
		public string LookupString;
		public List<GameObject> InactiveObjects = new List<GameObject>();

	// end of this class
}
