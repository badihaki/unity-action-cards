using System.Collections.Generic;
using UnityEngine;

public class CharacterEyesight : MonoBehaviour
{
	[field:SerializeField]
	public List<Transform> _NoticeableEntitiesInView { get; private set; }

	private void Start()
	{
		_NoticeableEntitiesInView = new List<Transform>();
	}

	private void OnTriggerEnter(Collider other)
	{
		EditViewList(other);
	}

	private void OnTriggerExit(Collider other)
	{
		EditViewList(other, false);
	}

	private void EditViewList(Collider other, bool isGoingIn = true)
	{
		//print($"{other.name} is in sight of {transform.parent.name}");
		switch (other.gameObject.layer)
		{
			case 3 or 6 or 8:
				if (isGoingIn)
				{
					_NoticeableEntitiesInView.Add(other.transform.parent.transform);
					if (GameManagerMaster.GameMaster.GMSettings.logExraPlayerData)
					{
						print($"in view {other.transform.parent.name}");
						TryTooSeeTarget(_NoticeableEntitiesInView.Count - 1);
					}
				}
				else
				{
					if (GameManagerMaster.GameMaster.GMSettings.logExraPlayerData)
						print($"trying to remove {other.transform.parent.name}");
					if (_NoticeableEntitiesInView.Contains(other.transform.parent.transform))
						_NoticeableEntitiesInView.Remove(other.transform.parent.transform);
				}
				break;
			default:
				break;
		}
	}

	public bool TryTooSeeTarget(int targetIndex)
	{
		if(targetIndex>0)
		{
			Vector3 rayDir = _NoticeableEntitiesInView[targetIndex].position - transform.parent.position;
			float rayDist = 18.5f;
			Ray ray = new Ray(transform.position, rayDir);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDist))
			{
				if(hitInfo.point == _NoticeableEntitiesInView[targetIndex].position)
				{
					print($"bingo!!!! can see the target!!!!! {_NoticeableEntitiesInView[targetIndex].name}");
					return true;
				}
			}
			Debug.DrawLine(transform.position, rayDir.normalized * rayDist, Color.yellow, rayDist * 2);
		}
		return false;
	}
}
