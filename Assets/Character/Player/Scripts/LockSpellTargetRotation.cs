using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSpellTargetRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotationCorrectionVector;
    [field: SerializeField] public Quaternion targetRotation { get; private set; }
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(rotationCorrectionVector);
        targetRotation = cam.transform.rotation * Quaternion.Euler(rotationCorrectionVector);
    }
}
