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
        // Quaternion rotation = transform.rotation;
        Quaternion rotation = Quaternion.Euler(-transform.forward);
        if (bloodFX != null)
        {
            Instantiate(bloodFX, transform.position, rotation);
        }
        if(_CurrentHealth <= 0)
        {
            GetComponent<IDestroyable>().DestroyEntity();
        }
    }

    // end
}
