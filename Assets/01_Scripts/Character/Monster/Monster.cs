using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIController))]
public class Monster : Character, IDamagable
{
    private Dictionary<Material, Color> materialTable;
    
    protected override void Awake()
    {
        base.Awake();

        Awake_InitMaterial();
    }

    private void Awake_InitMaterial()
    {
        materialTable = new Dictionary<Material, Color>();
        
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer r in renderers)
        {
            foreach (Material material in r.materials)
            {
                materialTable.Add(material, material.color);
            }
        }
    }
    
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
            
            StartCoroutine(Coroutine_SetDamagedColor(30));
        }

        StartCoroutine(Coroutine_StopAnimation(attacker, weaponData.HitStopFrame));
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

    private IEnumerator Coroutine_SetDamagedColor(int frame)
    {
        foreach (KeyValuePair<Material, Color> m in materialTable)
        {
            m.Key.color = Color.red;
        }

        yield return new WaitForSeconds(0.15f);
        
        foreach (KeyValuePair<Material, Color> m in materialTable)
        {
            m.Key.color = m.Value;
        }
    }

    private IEnumerator Coroutine_StopAnimation(GameObject attacker, int frame)
    {
        Animator attackerAnim = attacker.GetComponent<Animator>();
        {
            attackerAnim.speed = 0.0f;
        }

        animator.speed = 0.0f;
        
        for (int i = 0; i < frame; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        attackerAnim.speed = 1.0f;
        animator.speed = 1.0f;
    }
}
