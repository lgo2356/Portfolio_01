using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character, IDamagable
{
    [SerializeField]
    private Transform bodyTransform;

    private WeaponController weaponController;

    private void Reset()
    {
        GameObject go = GameObject.Find("Player");
        Debug.Assert(go != null);

        bodyTransform = go.transform.FindChildByName("Body");
        Debug.Assert(bodyTransform != null);
    }

    protected override void Awake()
    {
        base.Awake();

        Awake_GetComponent();
        Awake_BindInput();
    }

    private void Awake_GetComponent()
    {
        weaponController = GetComponent<WeaponController>();
    }

    private void Awake_BindInput()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputActionMap actionMap = playerInput.actions.FindActionMap("PlayerActions");

        actionMap.FindAction("Equip").started += (callback) =>
        {
            weaponController.Equip();
        };

        actionMap.FindAction("Action").started += (callback) =>
        {
            weaponController.DoAction();
        };
    }

    public void OnDamaged(GameObject attacker, Weapon causer, Vector3 hitPoint, WeaponData weaponData)
    {
        hpComponent.AddDamage(weaponData.Power);

        stateComponent.SetDamagedState();

        if (weaponData.HitParticle != null)
        {
            GameObject go = Instantiate(weaponData.HitParticle, transform, false);
            {
                go.transform.localPosition = hitPoint + weaponData.HitParticlePositionOffset;
                go.transform.localScale += weaponData.HitParticleScaleOffset;
            }
        }

        animator.SetInteger("ImpactType", (int)causer.Type);
        animator.SetInteger("ImpactIndex", weaponData.ImpactIndex);
        animator.SetTrigger("DoImpact");

        if (weaponData.LaunchDistance > 0f)
        {
            StartCoroutine(Coroutine_Launch(attacker, 30, weaponData.LaunchDistance));
        }
    }

    private IEnumerator Coroutine_Launch(GameObject attacker, int frame, float distance)
    {
        WaitForFixedUpdate waitForFixedUpdate = new();

        float launchDistance = rigidbody.drag * distance * 1000f;
        //rigidbody.AddForce(-transform.forward * launchDistance);
        rigidbody.AddForce(attacker.transform.forward * launchDistance);

        for (int i = 0; i < frame; i++)
        {
            yield return waitForFixedUpdate;
        }
    }
}
