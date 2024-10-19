using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AIController))]
public class Monster : Character, IDamagable
{
    public void OnDamaged(GameObject attacker, Weapon causer, Vector3 hitPoint, WeaponData weaponData)
    {
        hpComponent.AddDamage(weaponData.Power);

        if (weaponData.HitParticle != null)
        {
            GameObject go = Instantiate(weaponData.HitParticle, transform, false);
            go.transform.localPosition = hitPoint + weaponData.HitParticlePositionOffset;
            go.transform.localScale += weaponData.HitParticleScaleOffset;
        }

        if (hpComponent.IsDead)
        {
            stateComponent.SetDeadState();

            rigidbody.useGravity = false;

            Collider collider = GetComponent<Collider>();
            collider.enabled = false;

            animator.SetTrigger("DoDead");
        }
        else
        {
            stateComponent.SetDamagedState();

            transform.LookAt(attacker.transform, Vector3.up);

            animator.SetInteger("ImpactType", (int)causer.Type);
            animator.SetInteger("ImpactIndex", weaponData.ImpactIndex);
            animator.SetTrigger("DoImpact");

            if (weaponData.LaunchDistance > 0f)
            {
                StartCoroutine(Coroutine_Launch(30, weaponData.LaunchDistance));
            }
        }
    }

    private IEnumerator Coroutine_Launch(int frame, float distance)
    {
        WaitForFixedUpdate waitForFixedUpdate = new();

        float launchDistance = rigidbody.drag * distance * 1000f;
        rigidbody.AddForce(-transform.forward * launchDistance);

        for (int i = 0; i < frame; i++)
        {
            yield return waitForFixedUpdate;
        }
    }

    // 데미지 받았을 때 색깔
    private IEnumerator Coroutine_SetDamagedColor(int frame, float time)
    {
        WaitForFixedUpdate waitForFixedUpdate = new();

        for (int i = 0; i < frame; i++)
        {
            yield return waitForFixedUpdate;
        }
    }
}
