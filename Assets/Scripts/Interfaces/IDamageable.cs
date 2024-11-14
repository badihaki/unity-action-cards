using UnityEngine;

public interface IDamageable
{
    void Damage(int damage, Transform damageSource, float knockForce, float launchForce, Character damageSourceController);

    Transform GetControllingEntity();
    Transform GetDamagedEntity();
}
