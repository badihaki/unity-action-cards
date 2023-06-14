using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float _CurrentHealth { get; private set; }
    [SerializeField] private int _MaxHealth;

    public void InitiateHealth(int health)
    {
        _MaxHealth = health;
        ResetHealth();
    }

    public void ResetHealth()
    {
        _CurrentHealth = _MaxHealth;
    }

    // end
}
