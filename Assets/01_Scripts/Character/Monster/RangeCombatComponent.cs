using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangeCombatComponent : CombatComponent
{
    [Header("원거리")]
    [SerializeField]
    private float avoidDistance = 5.0f;

    private NavMeshAgent navMeshAgent;

    private Coroutine combatCoroutine;
    private Coroutine avoidCoroutine;

    private bool isCombat = true;
    private bool isAvoid = true;
    private bool isAvoiding;

    protected override void Awake()
    {
        base.Awake();

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override void StartCombat(GameObject target)
    {
        base.StartCombat(target);
        
        Debug.Assert(combatCoroutine == null, "Combat Coroutine은 중복 실행할 수 없습니다.");
        isCombat = true;
        combatCoroutine = StartCoroutine(Coroutine_Attack());
        
        Debug.Assert(avoidCoroutine == null, "Avoid Coroutine은 중복 실행할 수 없습니다.");
        isAvoid = true;
        avoidCoroutine = StartCoroutine(Coroutine_Avoid());

        isAvoiding = false;
    }

    public override void StopCombat()
    {
        base.StopCombat();

        Debug.Assert(combatCoroutine != null, "Combat Coroutine은 이미 실행 해제되었습니다.");
        StopCoroutine(combatCoroutine);
        combatCoroutine = null;
        isCombat = false;

        Debug.Assert(avoidCoroutine != null, "Avoid Coroutine은 이미 실행 해제되었습니다.");
        StopCoroutine(avoidCoroutine);
        avoidCoroutine = null;
        isAvoid = false;
    }

    private IEnumerator Coroutine_Attack()
    {
        while (isCombat)
        {
            if (Vector3.Distance(combatTarget.transform.position, transform.position) > attackDistance)
            {
                //TODO : 전투 대기
                
                transform.LookAt(combatTarget.transform);
            }
            else
            {
                if (canAttack)
                {
                    moveComponent.StopMove();
                    
                    weaponController.DoAction();
                    
                    float coolTime = GetAttackCoolTime();
                    StartCoroutine(Coroutine_SetCoolTime(coolTime));

                    yield return null;
                }
                else
                {
                    transform.LookAt(combatTarget.transform);
                }
            }
            
            //TODO : 플레이어 사망 체크하기
            
            yield return null;
        }
    }

    private IEnumerator Coroutine_Avoid()
    {
        while (isAvoid)
        {
            if (Vector3.Distance(combatTarget.transform.position, transform.position) <= avoidDistance)
            {
                print("Avoid!");
                
                animator.SetBool("IsAction", false);

                if (isAvoiding == false)
                {
                    animator.SetBool("IsWarpAction", true);
                }
            }
            
            yield return null;
        }
    }

    //TODO : 스킬 컴포넌트 개발하면 옮기기
    private void BeginAnimCast()
    {
        isAvoiding = true;
    }

    private void EndAnimCast()
    {
        isAvoiding = false;
        
        animator.SetBool("IsWarpAction", false);
        
        StartCoroutine(Coroutine_SetCoolTime(1.5f));
    }

    private void DoAnimCast()
    {
        Vector3 range = new()
        {
            x = UnityEngine.Random.Range(-3.0f, 3.0f),
            z = UnityEngine.Random.Range(-3.0f, 3.0f),
        };
        Vector3 newPosition = transform.position - transform.forward * 5.0f;
        newPosition += range;

        NavMeshPath newPath = new();

        if (navMeshAgent.CalculatePath(newPosition, newPath))
        {
            transform.position = newPosition;
        }
        else
        {
            print("그곳으로 이동할 수 없습니다.");
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, avoidDistance);
    }
}
