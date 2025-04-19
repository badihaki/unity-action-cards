using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable
{
    void CalculateKnockback(Transform forceSource, float knockforce, float launchForce);
}
