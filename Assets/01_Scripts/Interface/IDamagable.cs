using UnityEngine;

public interface IDamagable
{
    void OnDamaged(GameObject attacker, Weapon causer, Vector3 hitPoint, WeaponData weaponData);
    void OnDamaged(GameObject attacker, float damage);
}
