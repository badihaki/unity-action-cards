using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [field: SerializeField] public CharacterSheet CharacterSheet { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Health = GetComponent<Health>();
        Health.InitiateHealth(CharacterSheet.StartingHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
