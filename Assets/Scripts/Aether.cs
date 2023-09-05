using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aether : MonoBehaviour
{
    [field:SerializeField]public float _CurrentAetherPoints { get; private set; }
    [SerializeField] private int _MaxAetherPoints;

    public void InitiateAetherPointPool(int aetherPoints)
    {
        _MaxAetherPoints = aetherPoints;
        ResetAether();
    }

    public void ResetAether()
    {
        _CurrentAetherPoints = _MaxAetherPoints;
    }

    public void UseAetherPoints(int aetherPoints) => _CurrentAetherPoints -= aetherPoints;
    public void GainAetherPoints(int aetherPoints) => _CurrentAetherPoints += aetherPoints;
}
