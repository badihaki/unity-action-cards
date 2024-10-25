using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLockOnTargeter : MonoBehaviour
{
    [field: SerializeField] private PlayerCharacter player;
    [field: SerializeField] public List<Transform> meleeTargets;
    [field: SerializeField] public List<Transform> rangeTargets;

    public void Initialize(PlayerCharacter _player)
    {
        player = _player;
        PlayerTargetBox[] targetBoxChildren = GetComponentsInChildren<PlayerTargetBox>();
        foreach (PlayerTargetBox t in targetBoxChildren)
        {
            t.InitializeTargetBox(this);
        }
    }

    public void AddTargetToList(PlayerTargetBox.TargetType targetType, Transform target)
    {
        switch(targetType)
        {
            case PlayerTargetBox.TargetType.melee:
                meleeTargets.Add(target);
                break;
            case PlayerTargetBox.TargetType.range:
                rangeTargets.Add(target);
                break;
        }
    }

    public void RemoveTargetToList(PlayerTargetBox.TargetType targetType, Transform target)
    {
        switch (targetType)
        {
            case PlayerTargetBox.TargetType.melee:
                meleeTargets.Remove(target);
                break;
            case PlayerTargetBox.TargetType.range:
                rangeTargets.Remove(target);
                break;
        }
    }
}
