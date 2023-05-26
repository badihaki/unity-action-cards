using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float CurrentHealth { get; private set; }
    [SerializeField] private int MaxHealth;

    public void InitiateHealth(int health)
    {
        MaxHealth = health;
        ResetHealth();
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
    }

    // end
}
