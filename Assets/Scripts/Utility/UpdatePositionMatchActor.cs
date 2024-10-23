using UnityEngine;

public class UpdatePositionMatchActor : MonoBehaviour
{
    private Actor _Actor;
    public void Initialize(Actor actor)
    {
        _Actor = actor;
    }

    private void LateUpdate()
    {
        if (_Actor != null) transform.position = _Actor.transform.position;
	}
}
