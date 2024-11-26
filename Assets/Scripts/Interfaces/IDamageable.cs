using UnityEngine;

public interface IDamageable
{
    void Damage(int damage, Transform damageSource, bool knockedBack, bool launched, Character damageSourceController);

    Transform GetControllingEntity();
    Transform GetDamagedEntity();
}
