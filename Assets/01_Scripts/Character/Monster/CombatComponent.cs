using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MonsterMoveComponent))]
[RequireComponent(typeof(WeaponController))]
public class CombatComponent : MonoBehaviour
{
    [SerializeField]
    protected float attackDistance;

    [SerializeField]
    protected float attackCoolTime;

    [SerializeField]
    protected float attackCoolTimeDeviation;

    [SerializeField]
    protected float combatWalkSpeed;

    [SerializeField]
    protected WeaponType weaponType;

    [SerializeField]
    private bool isDebugging;

    protected virtual void Reset()
    {
        attackDistance = 2.0f;
        attackCoolTime = 3.0f;
        attackCoolTimeDeviation = 0.5f;
        combatWalkSpeed = 1.0f;
        weaponType = WeaponType.Unarmed;

        isDebugging = true;
    }

    protected Animator animator;
    protected new Rigidbody rigidbody;
    protected NavMeshAgent nevMeshAgent;
    protected StateComponent stateComponent;
    protected AIStateComponent aiStateComponent;
    protected MonsterMoveComponent moveComponent;
    protected WeaponController weaponController;

    protected GameObject combatTarget;
    protected Collider[] colliderBuffer;
    protected bool canAttack = true;
    protected float waitTime;
    
    protected Func<IEnumerator>[] waitCoroutines;

    public GameObject CombatTarget => combatTarget;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        nevMeshAgent = GetComponent<NavMeshAgent>();
        stateComponent = GetComponent<StateComponent>();
        aiStateComponent = GetComponent<AIStateComponent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
        weaponController = GetComponent<WeaponController>();
    }

    protected virtual void Start()
    {
        
    }

    public virtual void StartCombat(GameObject target)
    {
        if (combatTarget != null)
        {
            return;
        }

        print("Start Combat");
        
        combatTarget = target;
        
        weaponController.SetWeaponType(weaponType);
    }

    public virtual void StopCombat()
    {
        print("Stop Combat");

        combatTarget = null;
        
        weaponController.SetWeaponType(weaponType);
    }
    
    protected float GetAttackCoolTime()
    {
        float result = attackCoolTime;
        float deviation = UnityEngine.Random.Range(-attackCoolTimeDeviation, attackCoolTimeDeviation);

        result += deviation;
        
        return result;
    }
    
    protected IEnumerator Coroutine_SetCoolTime(float time)
    {
        canAttack = false;
        
        yield return new WaitForSeconds(time);

        canAttack = true;
    }
    
    protected IEnumerator Coroutine_MoveRight()
    {
        float time = 0.0f;

        moveComponent
            .SetLookTarget(combatTarget.transform)
            .SetDirection(MonsterMoveComponent.Direction.Right)
            .SetMoveSpeed(combatWalkSpeed);

        while (time < waitTime)
        {
            time += Time.deltaTime;

            if (stateComponent.IsDamagedState == false)
            {
                Vector3 dir = transform.right;
                Vector3 dest = transform.position + (dir * 0.5f);

                Debug.DrawRay(transform.position, dir * 0.5f, Color.magenta);

                moveComponent.SetDestination(dest);
            }
            
            yield return null;
        }
    }

    protected IEnumerator Coroutine_MoveLeft()
    {
        float time = 0.0f;

        moveComponent
            .SetLookTarget(combatTarget.transform)
            .SetDirection(MonsterMoveComponent.Direction.Left)
            .SetMoveSpeed(combatWalkSpeed * (-1.0f));

        while (time < waitTime)
        {
            time += Time.deltaTime;
            
            Vector3 dir = transform.right * (-1);
            Vector3 dest = transform.position + (dir * 0.5f);

            Debug.DrawRay(transform.position, dir * 0.5f, Color.magenta);

            moveComponent.SetDestination(dest);

            yield return null;
        }
    }

    protected float GetWaitTime(float min, float max)
    {
        float time = UnityEngine.Random.Range(min, max);
        
        return time;
    }
    
    protected int GetWaitCoroutineType()
    {
        int type = UnityEngine.Random.Range(0, waitCoroutines.Length);

        return type;
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        if (isDebugging == false)
            return;

        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, attackDistance);
        
        //if (Application.isPlaying == false)
        //    return;

        //if (combatTarget != null)
        //{
        //    Vector3 position = combatTarget.transform.position;
        //    position.y += 1.0f;
            
        //    Gizmos.DrawWireSphere(position, 1.5f);
        //}
    }
#endif
}
