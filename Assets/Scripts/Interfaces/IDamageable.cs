using UnityEngine;

public interface IDamageable
{
    void Damage(int damage, Transform damageSource, float knockForce, float launchForce);

    Transform GetControllingEntity();
    Transform GetDamagedEntity();
}
