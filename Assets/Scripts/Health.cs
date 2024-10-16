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

    [field: SerializeField] public float _MaxPoise { get; private set; } = 1.0f;
    [field: SerializeField] public float _PoiseFortification { get; private set; } = 0.0f;
    [field: SerializeField] public float _CurrentPoise { get; private set; }

    public delegate void GetHit(string hitType);
    public event GetHit OnHit;

    [SerializeField] private GameObject bloodFX;

    public void InitiateHealth(int health)
    {
        _MaxHealth = health;
        _MaxPoise = 1.0f;
        _CurrentPoise = _MaxPoise + _PoiseFortification;
        ResetHealth();
    }

    private void Update()
    {
        RestorePoise();
    }

    private void RestorePoise()
    {
        if (_CurrentPoise < _MaxPoise + _PoiseFortification)
        {
            float restoreRate;
            if (_CurrentPoise < (_MaxPoise + _PoiseFortification) / 2) restoreRate = 5.5f;
            else restoreRate = 2.35f;
            _CurrentPoise += Time.deltaTime * restoreRate;
        }
        else if (_CurrentPoise != _MaxPoise + _PoiseFortification) _CurrentPoise = _MaxPoise + _PoiseFortification;
    }

    public void ResetHealth()
    {
        _CurrentHealth = _MaxHealth;
        if (OnHealthChanged != null) OnHealthChanged(_CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        _CurrentHealth -= damage;
        CalculateHitResponse(damage);
        if (OnHealthChanged != null) OnHealthChanged(_CurrentHealth);
        
        // TODO: rotate blood
        // Quaternion rotation = transform.rotation;
        Quaternion rotation = Quaternion.Euler(-transform.forward);
        if (bloodFX != null)
        {
            GameObject blood = Instantiate(bloodFX, transform.position, rotation);
            blood.transform.rotation = transform.rotation;
        }
        if(_CurrentHealth <= 0)
        {
            GetComponent<IDestroyable>().DestroyEntity();
        }
    }

    public void FortifyPoise(float poise)
    {
        _PoiseFortification += poise;
    }

    private void CalculateHitResponse(float value)
    {
        float randomHit = UnityEngine.Random.Range(MathF.Abs(value) * 0.318f, value * MathF.PI / 2);
        _CurrentPoise -= randomHit;

        if (_CurrentPoise < 0 && _CurrentPoise > -0.5f)
        {
            if (OnHit != null) OnHit("hit");
        }
        else if(_CurrentPoise < -0.5f)
        {
            if (OnHit != null) OnHit("knockBack");
        }
    }
    // end
}
