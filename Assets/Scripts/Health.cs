using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float _CurrentHealth { get; private set; }
    [SerializeField] private int _MaxHealth;
    [SerializeField] private GameObject bloodFX;

    public void InitiateHealth(int health)
    {
        _MaxHealth = health;
        ResetHealth();
    }

    public void ResetHealth()
    {
        _CurrentHealth = _MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        _CurrentHealth -= damage;
        if (bloodFX != null)
        {
            print("bleedin");
            Instantiate(bloodFX, transform.position, Quaternion.identity, transform);
        }
        if(_CurrentHealth <= 0)
        {
            GetComponent<IDestroyable>().DestroyEntity();
        }
    }

    // end
}
