using System;
using System.Collections;
using UnityEngine;

public class MeleeCombatComponent : CombatComponent
{
    [Header("근접")]
    [SerializeField]
    private float combatDistance;

    protected override void Reset()
    {
        base.Reset();

        combatDistance = 10.0f;
    }

    private Vector3 combatPosition;
    
    private Coroutine combatCoroutine;
    private Coroutine checkCombatRangeCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        waitCoroutines = new Func<IEnumerator>[]
        {
            Coroutine_MoveRight,
            Coroutine_MoveLeft,
        };
    }

    public override void StartCombat(GameObject target)
    {
        base.StartCombat(target);
        
        combatPosition = transform.position;
        
        //checkCombatRangeCoroutine = StartCoroutine(Coroutine_CheckCombatRange());
        combatCoroutine = StartCoroutine(Coroutine_Attack());
    }

    public override void StopCombat()
    {
        base.StopCombat();

        //if (checkCombatRangeCoroutine != null)
        //{
        //    StopCoroutine(checkCombatRangeCoroutine);
        //    checkCombatRangeCoroutine = null;
        //}

        if (combatCoroutine != null)
        {
            StopCoroutine(combatCoroutine);
            combatCoroutine = null;
        }
        
        combatPosition = Vector3.zero;
    }
    
    private IEnumerator Coroutine_Attack()
    {
        while (true)
        {
            if (combatTarget == null)
            {
                break;
            }

            if (Vector3.Distance(combatTarget.transform.position, transform.position) > attackDistance)
            {
                // 공격 동작 중에 움직이는 버그 수정
                if (stateComponent.IsAttackState == false)
                {
                    moveComponent
                        .SetMoveSpeed(2.0f)
                        .SetLookTarget(combatTarget.transform)
                        .SetDirection(MonsterMoveComponent.Direction.Forward)
                        .SetDestination(combatTarget.transform.position);
                }
            }
            else
            {
                if (canAttack && stateComponent.IsDamagedState == false)
                {
                    moveComponent.StopMove();
                    transform.LookAt(combatTarget.transform);

                    animator.SetInteger("ActionType", (int)weaponType);
                    weaponController.DoAction();

                    float coolTime = GetAttackCoolTime();
                    StartCoroutine(Coroutine_SetCoolTime(coolTime));

                    yield return null;
                }

                if (stateComponent.IsAttackState == false && stateComponent.IsDamagedState == false)
                {
                    moveComponent.StopMove();
                    transform.LookAt(combatTarget.transform);

                    int waitType = GetWaitCoroutineType();
                    IEnumerator waitCoroutine = waitCoroutines[waitType]();
                    StartCoroutine(waitCoroutine);

                    waitTime = GetWaitTime(1.0f, 1.8f);
                    yield return new WaitForSeconds(waitTime);
                }

                //TODO : 플레이어 사망 체크하기
            }

            yield return null;
        }
    }
    
    private IEnumerator Coroutine_CheckCombatRange()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, combatPosition) > combatDistance)
            {
                print("Out of combat range");
                
                aiStateComponent.SetIdleState();
            }
            
            yield return null;
        }
    }
}
