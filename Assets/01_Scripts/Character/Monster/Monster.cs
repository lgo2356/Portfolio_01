using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIController))]
public class Monster : Character, IDamagable
{
    private WeaponController weaponController;

    private Dictionary<Material, Color> materialTable;

    public Action OnTauntAction;
    
    protected override void Awake()
    {
        base.Awake();

        weaponController = GetComponent<WeaponController>();

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
    
    public virtual void OnDamaged(GameObject attacker, Weapon causer, Vector3 hitPoint, WeaponData weaponData)
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
            OnDead();
        }
        else
        {
            transform.LookAt(attacker.transform, Vector3.up);

            animator.Play($"Blend {weaponController.currentType}", 0);

            if (animator.GetBool("IsAction"))
            {
                weaponController.EndAction();
            }

            animator.SetInteger("ImpactType", (int)causer.Type);
            animator.SetInteger("ImpactIndex", weaponData.ImpactIndex);
            animator.SetTrigger("DoImpact");

            if (weaponData.LaunchDistance > 0f)
            {
                StartCoroutine(Coroutine_Launch(attacker, 30, weaponData.LaunchDistance));
            }
            
            StartCoroutine(Coroutine_SetDamagedColor(0.15f));
        }

        StartCoroutine(Coroutine_StopAnimation(attacker, weaponData.HitStopFrame));
    }

    protected virtual void OnDead()
    {
        rigidbody.useGravity = false;

        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        animator.SetTrigger("DoDead");

        GetComponent<AIStateComponent>().SetDeadState();
    }

    private IEnumerator Coroutine_Launch(GameObject attacker, int frame, float distance)
    {
        WaitForFixedUpdate waitForFixedUpdate = new();

        float launchDistance = rigidbody.drag * distance * 1000f;
        rigidbody.AddForce(attacker.transform.forward * launchDistance);

        for (int i = 0; i < frame; i++)
        {
            yield return waitForFixedUpdate;
        }
    }

    private IEnumerator Coroutine_SetDamagedColor(float time)
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

    private void OnAnimTaunt()
    {
        OnTauntAction?.Invoke();
    }
}
