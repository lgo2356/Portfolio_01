using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponType = WeaponComponent.WeaponType;

[RequireComponent(typeof(MonsterMoveComponent))]
[RequireComponent(typeof(ChaseComponent))]
[RequireComponent(typeof(WeaponComponent))]
public class CombatComponent : MonoBehaviour
{
    [SerializeField]
    private float combatDistance = 10f;

    [SerializeField]
    private float attackDistance = 2.0f;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private WeaponType weaponType;

    private Animator animator;
    private ChaseComponent chaseComponent;
    private MonsterMoveComponent moveComponent;
    private WeaponComponent weaponComponent;

    private GameObject target;
    private Vector3 combatPosition;
    private Collider[] colliderBuffer;

    private void Reset()
    {
        layerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        chaseComponent = GetComponent<ChaseComponent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
        weaponComponent = GetComponent<WeaponComponent>();
    }

    public void StartCombat(GameObject target)
    {
        this.target = target;
        combatPosition = transform.position;

        weaponComponent.SetWeaponType(weaponType);

        StartCoroutine(Coroutine_Attack());
    }

    public void StopCombat()
    {
        
    }

    private IEnumerator Coroutine_Wait()
    {
        moveComponent.SetDestination(transform.position);

        while (true)
        {
            transform.LookAt(target.transform);

            if (Vector3.Distance(moveComponent.Destination, transform.position) < 0.1f)
            {
                moveComponent
                    .SetMoveSpeed(0.8f)
                    .SetDestination(GetRandomPosition())
                    .StartMove();
            }

            yield return null;
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = UnityEngine.Random.Range(-0.5f, 0.5f);
        float z = UnityEngine.Random.Range(-0.3f, 0.3f);

        Vector3 delta = new(x, 0, z);

        return transform.position + delta;
    }

    private IEnumerator Coroutine_Attack()
    {
        while (true)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > attackDistance)
            {
                //TODO : Chase

                chaseComponent.StartChase(target);
            }
            else
            {
                //TODO : Attack

                chaseComponent.StopChase();

                animator.SetInteger("ActionType", 1);
                weaponComponent.DoAction();

                //yield return new WaitForSeconds(0.1f);

                //weaponComponent.DoAction();

                //yield return new WaitForSeconds(0.4f);

                //weaponComponent.DoAction();

                break;
            }

            yield return null;
        }
    }
}
