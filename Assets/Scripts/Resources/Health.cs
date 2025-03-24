using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Health : MonoBehaviour
{
    [field: SerializeField] public int _CurrentHealth { get; private set; }
    [SerializeField] public int _MaxHealth { get; private set; }
    public bool restoringPoise;

    public delegate void ChangeHealth(int health);
    public event ChangeHealth OnHealthChanged;

    /*
    // >>>>>>>> POISE
     * Essentially, 
     */
    [field: SerializeField] public float _MaxPoise { get; private set; } = 1.0f;
    [field: SerializeField] public float _PoiseFortification { get; private set; } = 0.0f;
    [field: SerializeField] public float _CurrentPoise { get; private set; }
    [SerializeField] public float _PoiseThreshold() => _MaxPoise + _PoiseFortification;

    public delegate void GetHit(string hitType);
    public event GetHit OnHit;


    public void InitiateHealth(int health)
    {
        _MaxHealth = health;
        _MaxPoise = 1.0f;
        _CurrentPoise = 0;
        restoringPoise = false;
        ResetHealth();
    }

    private IEnumerator RestorePoise()
    {
        while (restoringPoise)
        {
            if (_CurrentPoise > 0) // max poise and poise fort creates the poise threshold.
            {
                float restoreRate;
                if (_CurrentPoise > (_PoiseThreshold()) / 2) restoreRate = 1.35f;
                else restoreRate = 0.735f;
                _CurrentPoise -= Time.deltaTime * restoreRate;
                yield return null;
            }
            else
            {
                _CurrentPoise = 0;
                restoringPoise = false;
            }
        }
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
            TryGetComponent(out IDestroyable destroyable);
            destroyable?.DestroyEntity();
            //GetComponent<IDestroyable>().DestroyEntity();
        }
    }

    public float ChangePoise(float poise)
    {
        //_PoiseFortification += poise;
        _CurrentPoise += poise;
        if (_CurrentPoise > _PoiseThreshold())
        {
            restoringPoise = true;
            StartCoroutine(RestorePoise());
        }
		return _CurrentPoise;
    }

    // end
}
