using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MonsterMoveComponent))]
[RequireComponent(typeof(ChaseComponent))]
[RequireComponent(typeof(WeaponController))]
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
    private WeaponType weaponType;

    private Animator animator;
    private AIStateComponent stateComponent;
    private MonsterMoveComponent moveComponent;
    private WeaponController weaponController;

    private GameObject combatTarget;
    private Vector3 combatPosition;
    private Collider[] colliderBuffer;
    
    private Coroutine combatCoroutine;
    private Coroutine checkCombatRangeCoroutine;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        stateComponent = GetComponent<AIStateComponent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
        weaponController = GetComponent<WeaponController>();
    }

    private void Start()
    {
        Start_BindEvent();
    }

    private void Start_BindEvent()
    {
        #region Weapon
        weaponController.OnAnimEquipEnd += () =>
        {

        };

        weaponController.OnAnimActionEnd += () =>
        {

        };
        #endregion
    }

    public void StartCombat(GameObject target)
    {
        print("Start Combat");
        
        combatTarget = target;
        combatPosition = transform.position;

        weaponController.SetWeaponType(weaponType);
        
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
            if (Vector3.Distance(combatTarget.transform.position, transform.position) > attackDistance)
            {
                moveComponent
                    .SetMoveSpeed(2.0f)
                    .SetDestination(combatTarget.transform.position);
            }
            else
            {
                moveComponent.StopMove();
                
                transform.LookAt(combatTarget.transform);

                animator.SetInteger("ActionType", 1);
                weaponController.DoAction();
                
                //TODO : 플레이어 사망 체크하기

                float coolTime = GetAttackCoolTime();

                yield return new WaitForSeconds(coolTime);
            }

            yield return null;
        }
    }

    private float GetAttackCoolTime()
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
            transform.LookAt(combatTarget.transform);

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
