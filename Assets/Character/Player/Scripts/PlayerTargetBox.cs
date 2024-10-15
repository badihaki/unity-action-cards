using UnityEngine;

public class PlayerTargetBox : MonoBehaviour
{
    public enum TargetType
    {
        melee,
        range,
    }
    public TargetType type;

    private PlayerLockOnTargeter targeter;
    private bool ready = false;

    public void InitializeTargetBox(PlayerLockOnTargeter _targeter)
    {
        targeter = _targeter;
        ready = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ready)
        {
            ITargetable target = other.GetComponent<ITargetable>();
            if (target != null)
            {
                /*if (!targeter.Contains(target.GetTargetable()))
                {
                }*/
                if (!DoesContainInAnyList(transform))
                {
                    targeter.AddTargetToList(type, target.GetTargetable());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ready)
        {
            ITargetable target = other.GetComponent<ITargetable>();
            if (target != null)
            {
                /*if (targets.Contains(target.GetTargetable()))
                {
                    targets.Remove(target.GetTargetable());
                }*/
                if (!DoesContainInAnyList(transform))
                {
                    targeter.RemoveTargetToList(type, target.GetTargetable());
                }
            }
        }
    }

    bool DoesContainInAnyList(Transform target)
    {
        switch (type)
        {
            case TargetType.melee:
                if(targeter.meleeTargets.Contains(target)) return true;
                break;
            case TargetType.range:
                if (targeter.rangeTargets.Contains(target)) return true;
                break;
        }
        return false;
    }
}
