using UnityEngine;

public class Boss : Monster
{
    public override void OnDamaged(GameObject attacker, Weapon causer, Vector3 hitPoint, WeaponData weaponData)
    {
        base.OnDamaged(attacker, causer, hitPoint, weaponData);
    }

    protected override void OnDead()
    {
        base.OnDead();

        GetComponent<AIStateComponent>().SetDeadState();
    }

    public void SetTaunt()
    {
        animator.SetTrigger("DoTaunt");
    }
}
