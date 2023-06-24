using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSpellTargetRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotationCorrectionVector;
    [field: SerializeField] public Quaternion targetRotation { get; private set; }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Camera.main.transform.rotation * Quaternion.Euler(rotationCorrectionVector);
        // transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(rotationCorrectionVector);
    }
}
