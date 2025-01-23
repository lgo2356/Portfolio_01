using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : Character, IDamagable
{
    [SerializeField]
    private Transform bodyTransform;

    private WeaponController weaponController;
    private SkillController skillController;

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

        weaponController = GetComponent<WeaponController>();
        skillController = GetComponent<SkillController>();

        Awake_BindInput();
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


        actionMap.FindAction("Skill").started += (callback) =>
        {
            skillController.OnPressedDown();
        };

        actionMap.FindAction("Skill").performed += (callback) =>
        {
            switch (callback.interaction)
            {
                case TapInteraction:
                {
                    skillController.OnPerformedShort();
                }
                break;

                case HoldInteraction:
                {
                    skillController.OnPerformedLong();
                }
                break;
            }
        };

        actionMap.FindAction("Skill").canceled += (callback) =>
        {
            Debug.Log("Cancel");

            skillController.OnPressedUp();
        };
    }

    public void OnDamaged(GameObject attacker, Weapon causer, Vector3 hitPoint, WeaponData weaponData)
    {
        hpComponent.AddDamage(weaponData.Power);

        if (weaponData.HitParticle != null)
        {
            GameObject go = Instantiate(weaponData.HitParticle, transform, false);
            {
                go.transform.localPosition = hitPoint + weaponData.HitParticlePositionOffset;
                go.transform.localScale += weaponData.HitParticleScaleOffset;
            }
        }

        if (stateComponent.GetSubType(StateComponent.SubStateType.Toughness) == false)
        {
            animator.Play($"Blend {weaponController.currentType}", 0);
            animator.SetInteger("ImpactType", (int)causer.Type);
            animator.SetInteger("ImpactIndex", weaponData.ImpactIndex);
            animator.SetTrigger("DoImpact");
        }

        if (animator.GetBool("IsAction"))
        {
            weaponController.EndAction();
        }

        if (weaponData.LaunchDistance > 0f)
        {
            StartCoroutine(Coroutine_Launch(attacker, 30, weaponData.LaunchDistance));
        }
    }

    public virtual void OnDamaged(GameObject attacker, int causerType, WeaponData weaponData)
    {
        
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
}
