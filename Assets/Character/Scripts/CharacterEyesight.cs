using System.Collections.Generic;
using UnityEngine;

public class CharacterEyesight : MonoBehaviour
{
	public List<Transform> _NoticeableEntitiesInView { get; private set; } = new List<Transform>();

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
			case 3:
			case 6:
			case 8:
				if (isGoingIn)
				{
					_NoticeableEntitiesInView.Add(other.transform);
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
					if (_NoticeableEntitiesInView.Contains(other.transform))
						_NoticeableEntitiesInView.Remove(other.transform);
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
			Vector3 rayDir = _NoticeableEntitiesInView[targetIndex].position - transform.position;
			Ray ray = new Ray(transform.position, rayDir);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, 12.5f))
			{
				print(hitInfo.point);
				if(hitInfo.point == _NoticeableEntitiesInView[targetIndex].position)
				{
					print($"bingo!!!! can see the target!!!!! {_NoticeableEntitiesInView[targetIndex].name}");
				}
			}
		}
		return false;
	}
}
