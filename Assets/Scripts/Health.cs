using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public int _CurrentHealth { get; private set; }
    [SerializeField] public int _MaxHealth { get; private set; }

    public delegate void ChangeHealth(int health);
    public event ChangeHealth OnHealthChanged;

    [SerializeField] private GameObject bloodFX;

    public void InitiateHealth(int health)
    {
        _MaxHealth = health;
        ResetHealth();
    }

    public void ResetHealth()
    {
        _CurrentHealth = _MaxHealth;
        if (OnHealthChanged != null) OnHealthChanged(_CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        _CurrentHealth -= damage;
        if (OnHealthChanged != null) OnHealthChanged(_CurrentHealth);
        
        // TODO: rotate blood
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
