using System;
using System.Collections;
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
    private float attackCoolTime = 2.0f;

    [SerializeField]
    private float attackCoolTimeDeviation = 0.5f;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private WeaponType weaponType;

    private Animator animator;
    private AIStateComponent stateComponent;
    private MonsterMoveComponent moveComponent;
    private WeaponComponent weaponComponent;

    private GameObject target;
    private Vector3 combatPosition;
    private Collider[] colliderBuffer;
    
    private Coroutine combatCoroutine;
    private Coroutine checkCombatRangeCoroutine;

    private void Reset()
    {
        layerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stateComponent = GetComponent<AIStateComponent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
        weaponComponent = GetComponent<WeaponComponent>();
    }

    private void Start()
    {
        Start_BindEvent();
    }

    private void Start_BindEvent()
    {
        #region Weapon
        weaponComponent.OnAnimEquipEnd += () =>
        {

        };

        weaponComponent.OnAnimActionEnd += () =>
        {

        };
        #endregion
    }

    public void StartCombat(GameObject target)
    {
        print("Start Combat");
        
        this.target = target;
        combatPosition = transform.position;

        weaponComponent.SetWeaponType(weaponType);
        
        checkCombatRangeCoroutine = StartCoroutine(Coroutine_CheckCombatRange());
        combatCoroutine = StartCoroutine(Coroutine_Attack());
    }

    public void StopCombat()
    {
        print("Stop Combat");
        
        StopCoroutine(checkCombatRangeCoroutine);
        checkCombatRangeCoroutine = null;
        
        StopCoroutine(combatCoroutine);
        combatCoroutine = null;
    }

    private IEnumerator Coroutine_Attack()
    {
        while (true)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > attackDistance)
            {
                //TODO : Chase

                moveComponent
                    .SetMoveSpeed(2.0f)
                    .SetDestination(target.transform.position);

                //chaseComponent.StartChase(target);
            }
            else
            {
                //TODO : Attack

                //chaseComponent.StopChase();

                moveComponent.StopMove();
                
                transform.LookAt(target.transform);

                animator.SetInteger("ActionType", 1);
                weaponComponent.DoAction();

                float coolTime = GetCoolTime();

                yield return new WaitForSeconds(coolTime);
            }

            yield return null;
        }
    }

    private float GetCoolTime()
    {
        float result = attackCoolTime;
        float deviation = UnityEngine.Random.Range(-attackCoolTimeDeviation, attackCoolTimeDeviation);

        result += deviation;
        
        return result;
    }

    private IEnumerator Coroutine_CheckCombatRange()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, combatPosition) > combatDistance)
            {
                print("Out of combat range");
                
                stateComponent.SetIdleState();
            }
            
            yield return null;
        }
    }

    private IEnumerator Coroutine_Wait()
    {
        moveComponent.SetDestination(transform.position);

        while (true)
        {
            transform.LookAt(target.transform);

            if (Vector3.Distance(moveComponent.Destination, transform.position) < 0.1f)
            {
                moveComponent.StartMove(GetRandomPosition(), 0.8f);
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
}
