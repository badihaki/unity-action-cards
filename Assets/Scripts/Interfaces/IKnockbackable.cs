using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable
{
    void ApplyKnockback(Transform forceSource, float knockforce, float launchForce);
}
