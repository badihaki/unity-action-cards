using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aether : MonoBehaviour
{
    [field: SerializeField, Header("Values")] public int _CurrentAether { get; private set; }
    [field: SerializeField] public int _MaxAether { get; private set; }

	[field: SerializeField, Header("Recovery values")] private bool canRecoverAether;
	[field: SerializeField] public float _RecoveryRate { get; private set; }
	[field: SerializeField] public float _RecoveryTimeNeeded { get; private set; } = 1.3f;
	[field: SerializeField] public int _RecoveryAmount { get; private set; } = 3;
	[field: SerializeField] public WaitForSeconds _RecoveryWaitTime { get; private set; } = new WaitForSeconds(1.5f);

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

		if (!canRecoverAether)
		{
			canRecoverAether = true;
			StartCoroutine(StartAetherRecovery());
		}
	}

	public void RestoreAether(int value)
	{
		_CurrentAether += value;
		EmitAetherValue();
	}

	private IEnumerator StartAetherRecovery()
	{
		yield return _RecoveryWaitTime;
		StartCoroutine(RecoverAether());
	}

	private IEnumerator RecoverAether()
	{
		while(canRecoverAether)
		{
			_RecoveryRate += Time.deltaTime;
			if(_RecoveryRate >= _RecoveryTimeNeeded)
			{
				_RecoveryRate = 0.00f;
				RestoreAether(_RecoveryAmount);
				if(_CurrentAether  >= _MaxAether)
				{
					_CurrentAether = _MaxAether;
					canRecoverAether = false;
				}
			}
			yield return null;
		}
	}

	private void EmitAetherValue()
	{
		if (OnAetherChanged != null) OnAetherChanged(_CurrentAether);
	}
}
