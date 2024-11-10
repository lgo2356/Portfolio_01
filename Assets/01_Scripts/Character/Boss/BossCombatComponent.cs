using System;
using System.Collections;
using UnityEngine;

public class BossCombatComponent : CombatComponent
{
    [SerializeField, Header("보스")]
    private float backwardDistance = 2.0f;
    
    private Coroutine combatCoroutine;
    private Func<IEnumerator>[] waitCoroutines;
    private float waitTime;

    protected override void Start()
    {
        base.Start();
        
        /**
         * 코루틴의 인스턴스는 한번 실행되고 완료되면 종료 상태가 되기 때문에 다시 실행할 수 없다.
         * 코루틴을 다시 실행하려면 새로 인스턴스를 생성해야 한다.
         */
        waitCoroutines = new Func<IEnumerator>[]
        {
            Coroutine_MoveBackward,
            Coroutine_MoveRight,
            Coroutine_MoveLeft,
        };
    }

    public override void StartCombat(GameObject target)
    {
        base.StartCombat(target);

        combatCoroutine = StartCoroutine(Coroutine_Combat());
    }

    public override void StopCombat()
    {
        base.StopCombat();
        
        StopCoroutine(combatCoroutine);
        combatCoroutine = null;
    }

    private IEnumerator Coroutine_Combat()
    {
        while (true)
        {
            // 뛰어서 접근할 거리 판단
            if (Vector3.Distance(combatTarget.transform.position, transform.position) > attackDistance)
            {
                moveComponent
                    .SetMoveSpeed(2.0f)
                    .SetLookTarget(combatTarget.transform)
                    .SetDirection(MonsterMoveComponent.Direction.Forward)
                    .SetDestination(combatTarget.transform.position);
            }
            else
            {
                // if (canAttack)
                // {
                //     animator.SetInteger("AttackType", GetRandomAttackType());
                //     weaponController.DoAction();
                //     
                //     float coolTime = GetAttackCoolTime();
                //     StartCoroutine(Coroutine_SetCoolTime(coolTime));
                //     
                //     yield return new WaitForSeconds(coolTime);
                // }
                
                moveComponent.StopMove();
                transform.LookAt(combatTarget.transform);

                waitTime = GetWaitTime();
                int waitType = GetWaitCoroutineType();
                IEnumerator waitCoroutine = waitCoroutines[waitType]();
                StartCoroutine(waitCoroutine);
                
                yield return new WaitForSeconds(waitTime);
            }
            
            yield return null;
        }
    }
    
    private IEnumerator Coroutine_MoveBackward()
    {
        float time = 0.0f;

        moveComponent
            .SetLookTarget(combatTarget.transform)
            .SetDirection(MonsterMoveComponent.Direction.Backward)
            .SetMoveSpeed(-1.0f);

        while (time < waitTime)
        {
            time += Time.deltaTime;
            
            Vector3 dir = transform.forward * (-1.0f);
            Vector3 dest = transform.position + (dir * 1.0f);

            Debug.DrawRay(transform.position, dir * 1.0f, Color.magenta);

            moveComponent.SetDestination(dest);
            
            yield return null;
        }
    }

    private IEnumerator Coroutine_MoveRight()
    {
        float time = 0.0f;

        moveComponent
            .SetLookTarget(combatTarget.transform)
            .SetDirection(MonsterMoveComponent.Direction.Right)
            .SetMoveSpeed(1.0f);

        while (time < waitTime)
        {
            time += Time.deltaTime;
            
            Vector3 dir = transform.right;
            Vector3 dest = transform.position + (dir * 1.0f);

            Debug.DrawRay(transform.position, dir * 1.0f, Color.magenta);

            moveComponent.SetDestination(dest);
            
            yield return null;
        }
    }

    private IEnumerator Coroutine_MoveLeft()
    {
        float time = 0.0f;

        moveComponent
            .SetLookTarget(combatTarget.transform)
            .SetDirection(MonsterMoveComponent.Direction.Left)
            .SetMoveSpeed(-1.0f);

        while (time < waitTime)
        {
            time += Time.deltaTime;
            
            Vector3 dir = transform.right * (-1);
            Vector3 dest = transform.position + (dir * 1.0f);

            Debug.DrawRay(transform.position, dir * 1.0f, Color.magenta);

            moveComponent.SetDestination(dest);

            yield return null;
        }
    }

    private int GetRandomAttackType()
    {
        int type = UnityEngine.Random.Range(1, 4);

        return type;
    }

    private float GetWaitTime()
    {
        float time = UnityEngine.Random.Range(1.5f, 3.0f);
        
        return time;
    }

    private int GetWaitCoroutineType()
    {
        int type = UnityEngine.Random.Range(0, waitCoroutines.Length);

        return type;
    }
}
