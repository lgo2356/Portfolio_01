using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MonsterMoveComponent))]
[RequireComponent(typeof(ChaseComponent))]
[RequireComponent(typeof(ObjectScanComponent))]
public class Monster : Character, IDamagable
{
    private MonsterMoveComponent moveComponent;
    private ChaseComponent chaseComponent;
    private ObjectScanComponent playerScanComponent;

    protected override void Awake()
    {
        base.Awake();

        moveComponent = GetComponent<MonsterMoveComponent>();
        chaseComponent = GetComponent<ChaseComponent>();
        playerScanComponent = GetComponent<ObjectScanComponent>();
    }

    protected override void Start()
    {
        base.Start();

        GameObject destObject = GameObject.Find("Destination");

        //chaseComponent.StartChase(destObject);

        Start_BindEvent();
    }

    private void Start_BindEvent()
    {
        playerScanComponent.OnFoundAction += (found) =>
        {
            //TODO : Start chasing player
            print($"Found : {found.name}");

            chaseComponent.StartChase(found);
        };

        playerScanComponent.OnLostAction += (lost) =>
        {
            //TODO : Stop chasing player
            print($"Lost : {lost.name}");

            chaseComponent.StopChase();
        };
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

            animator.SetInteger("ImpactType", (int)causer.WeaponType);
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

        float launchDistance = rigidbody.drag * distance * 10000f;
        rigidbody.AddForce(-transform.forward * launchDistance);

        for (int i = 0; i < frame; i++)
        {
            yield return waitForFixedUpdate;
        }
    }
}
