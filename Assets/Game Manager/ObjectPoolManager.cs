using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
	[SerializeField]
	public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
	public enum PoolFolder
	{
		None,
		Character,
		Projectile,
		Particle,
	}
	public static PoolFolder IntendedPoolFolder;

	public static GameObject GetObjectFromPool(GameObject objToSpwn, Vector3 spwnPos, Quaternion spwnRot, PoolFolder poolFolder = PoolFolder.None) // regulr
	{
		PooledObjectInfo pool = ObjectPools.Find(pool => pool.LookupString == objToSpwn.name);
		// if the pool doesnt exist
		if (pool == null)
			pool = CreateNewObjPool(objToSpwn);

		// check if there are any inactive objects
		GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

		if (spawnableObject == null) // we make a new one
		{
			spawnableObject = CreateNewObject(objToSpwn, spwnPos, spwnRot, poolFolder);
		}
		else // we reset its pos/rot, remove it from the list, set active to true
		{
			SpawnObjectInWorld(spwnPos, spwnRot, pool, spawnableObject);
		}

		return spawnableObject;
	}
	public static GameObject GetObjectFromPool(GameObject objToSpwn, Vector3 spwnPos, Quaternion spwnRot, Transform parentTransform, PoolFolder poolFolder = PoolFolder.None) // with parent
	{
		PooledObjectInfo pool = ObjectPools.Find(pool => pool.LookupString == objToSpwn.name);
		// if the pool doesnt exist
		if (pool == null)
			pool = CreateNewObjPool(objToSpwn);

		// check if there are any inactive objects
		GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

		if (spawnableObject == null) // we make a new one
		{
			spawnableObject = CreateNewObject(objToSpwn, spwnPos, spwnRot, poolFolder);
		}
		else // we reset its pos/rot, remove it from the list, set active to true
		{
			SpawnObjectInWorld(spwnPos, spwnRot, pool, spawnableObject);
		}
		spawnableObject.transform.SetParent(parentTransform);
		return spawnableObject;
	}
	public static GameObject GetObjectFromPool(GameObject objToSpwn, Vector3 spwnPos, Quaternion spwnRot, PoolFolder poolFolder = PoolFolder.None, string desiredName = "") // with a new name
	{
		PooledObjectInfo pool = ObjectPools.Find(pool => pool.LookupString == objToSpwn.name);
		// if the pool doesnt exist
		if (pool == null)
			pool = CreateNewObjPool(objToSpwn, desiredName);

		// check if there are any inactive objects
		GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

		if (spawnableObject == null) // we make a new one
		{
			spawnableObject = CreateNewObject(objToSpwn, spwnPos, spwnRot, poolFolder, desiredName);
		}
		else // we reset its pos/rot, remove it from the list, set active to true
		{
			SpawnObjectInWorld(spwnPos, spwnRot, pool, spawnableObject);
		}

		return spawnableObject;
	}

	public static void ReturnObjectToPool(GameObject obj)
	{
		//string gameObjName = obj.name.Replace("(Clone)", string.Empty);
		PooledObjectInfo pool = ObjectPools.Find(pool => pool.LookupString == obj.name);

		if (pool == null)
		{
			Debug.LogError($"Trying to return an object to a pool that doesn't exist?! Object is {obj.ToString()}");
			return;
		}
		obj.SetActive(false);
		pool.InactiveObjects.Add(obj);
	}

	private static void SpawnObjectInWorld(Vector3 spwnPos, Quaternion spwnRot, PooledObjectInfo pool, GameObject spawnableObject)
	{
		spawnableObject.transform.position = spwnPos;
		spawnableObject.transform.rotation = spwnRot;
		pool.InactiveObjects.Remove(spawnableObject);
		spawnableObject.SetActive(true);
	}

	private static GameObject CreateNewObject(GameObject objToSpwn, Vector3 spwnPos, Quaternion spwnRot, PoolFolder poolFolder)
	{
		GameObject spawnableObject = Instantiate(objToSpwn, spwnPos, spwnRot);
		string objectName = spawnableObject.name.Replace("(Clone)", string.Empty);
		spawnableObject.name = objectName;
		// set the parent
		Transform parentObj = SetParentObject(poolFolder);
		if (parentObj != null)
		{
			spawnableObject.transform.SetParent(parentObj);
		} // /
		return spawnableObject;
	}
	private static GameObject CreateNewObject(GameObject objToSpwn, Vector3 spwnPos, Quaternion spwnRot, PoolFolder poolFolder, string newName) // with a new name
	{
		GameObject spawnableObject = Instantiate(objToSpwn, spwnPos, spwnRot);
		spawnableObject.name = newName;
		// set the parent
		Transform parentObj = SetParentObject(poolFolder);
		if (parentObj != null)
		{
			spawnableObject.transform.SetParent(parentObj);
		} // /
		return spawnableObject;
	}

	private static PooledObjectInfo CreateNewObjPool(GameObject objToSpwn)
	{
		// setup a new pool
		PooledObjectInfo pool = new PooledObjectInfo() { LookupString = objToSpwn.name };
		ObjectPools.Add(pool);
		return pool;
	}
	private static PooledObjectInfo CreateNewObjPool(GameObject objToSpwn, string customName) // with a specified name
	{
		// setup a new pool
		PooledObjectInfo pool = new PooledObjectInfo() { LookupString = customName };
		ObjectPools.Add(pool);
		return pool;
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

[Serializable]
public class PooledObjectInfo
{
	public string LookupString;
	public List<GameObject> InactiveObjects = new List<GameObject>();
	//public ObjectPool<GameObject> pool;
	// end of this class
}
