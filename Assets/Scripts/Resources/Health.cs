using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Health : MonoBehaviour
{
    [field: SerializeField] public int _CurrentHealth { get; private set; }
    [SerializeField] public int _MaxHealth { get; private set; }

    public delegate void ChangeHealth(int health);
    public event ChangeHealth OnHealthChanged;

    /*
    // >>>>>>>> POISE
     * Essentially, 
     */
    [field: SerializeField] public float _MaxPoise { get; private set; } = 1.0f;
    [field: SerializeField] public float _PoiseFortification { get; private set; } = 0.0f;
    [field: SerializeField] public float _CurrentPoise { get; private set; }
    [SerializeField] public float _PoiseThreshold() => _CurrentPoise + _PoiseFortification;

    public delegate void GetHit(string hitType);
    public event GetHit OnHit;


    public void InitiateHealth(int health)
    {
        _MaxHealth = health;
        _MaxPoise = 3.0f;
        _CurrentPoise = 0;
        ResetHealth();
    }

    private void Update()
    {
        RestorePoise();
    }

    private void RestorePoise()
    {
        if (_CurrentPoise > 0) // max poise and poise fort creates the poise threshold.
        {
            float restoreRate;
            if (_CurrentPoise > (_PoiseThreshold()) / 2) restoreRate = 1.35f;
            else restoreRate = 0.735f;
            _CurrentPoise -= Time.deltaTime * restoreRate;
        }
        else _CurrentPoise = 0;
    }

    public void ResetHealth()
    {
        _CurrentHealth = _MaxHealth;
        if (OnHealthChanged != null) OnHealthChanged(_CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        _CurrentHealth -= damage;
        //CalculateHitResponse(damage);
        if (OnHealthChanged != null) OnHealthChanged(_CurrentHealth);
        
        // TODO: rotate blood
        // Quaternion rotation = transform.rotation;
        Quaternion rotation = Quaternion.Euler(-transform.forward);

        if(_CurrentHealth <= 0)
        {
            GetComponent<IDestroyable>().DestroyEntity();
        }
    }

    public float ChangePoise(float poise)
    {
        //_PoiseFortification += poise;
        _CurrentPoise += poise;
		return poise;
    }

    public void EmitOnHit(string hitType)=> OnHit?.Invoke(hitType);
    // end
}
