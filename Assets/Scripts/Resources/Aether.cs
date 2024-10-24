using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aether : MonoBehaviour
{
    [field: SerializeField] public int _CurrentAether { get; private set; }
    [field: SerializeField] public int _MaxAether { get; private set; }

    public delegate void ChangeAether(int aether);
    public event ChangeAether OnAetherChanged;

    public void InitiateAetherPointPool(int aetherPoints)
    {
        _MaxAether = aetherPoints;
		_CurrentAether = _MaxAether;
        ResetAether();
    }

    public void ResetAether()
    {
        _CurrentAether = _MaxAether;
		EmitAetherValue();
	}

	public void UseAether(int value)
    {
        _CurrentAether -= value;
		EmitAetherValue();
	}

	public void RestoreAether(int value)
	{
		_CurrentAether += value;
		EmitAetherValue();
	}

	private void EmitAetherValue()
	{
		if (OnAetherChanged != null) OnAetherChanged(_CurrentAether);
	}
}
