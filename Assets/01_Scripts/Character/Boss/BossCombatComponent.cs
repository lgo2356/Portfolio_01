using System;
using System.Collections;
using UnityEngine;

public class BossCombatComponent : CombatComponent
{
    [SerializeField, Header("보스")]
    private float backwardDistance = 2.0f;
    
    private Coroutine combatCoroutine;

    public int AttackType = 1;

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
                    .SetDestination(combatTarget.transform.position);
            }
            
            //  
            
            else
            {
                moveComponent.StopMove();
                
                transform.LookAt(combatTarget.transform);
                
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

                // MoveBackward();
                // MoveRight();
                // MoveLeft();

                StartCoroutine(Coroutine_MoveLeft(1.0f));
                
                yield return new WaitForSeconds(5.0f);
            }
            
            yield return null;
        }
    }

    private void MoveBackward()
    {
        Vector3 dest = transform.position + (-transform.forward * 3.0f);
        
        moveComponent
            .SetLookTarget(combatTarget.transform)
            .SetMoveSpeed(-1.0f)
            .SetDestination(dest);
    }

    private void MoveRight()
    {
        Vector3 dest = transform.position + (transform.right * 3.0f);
        
        moveComponent
            .SetLookTarget(combatTarget.transform)
            .SetMoveSpeed(1.0f)
            .SetDestination(dest);
    }

    private IEnumerator Coroutine_MoveLeft(float duration)
    {
        float time = 0.0f;

        moveComponent
            .SetLookTarget(combatTarget.transform)
            .SetMoveSpeed(-1.0f);
        
        while (time < duration)
        {
            time += Time.deltaTime;
            
            Vector3 dir = Vector3.Cross(combatTarget.transform.position - transform.position, combatTarget.transform.up);
            Vector3 dest = transform.position + (dir.normalized * 1.0f);
            
            Debug.DrawRay(transform.position, dest, Color.magenta);
        
            moveComponent.SetDestination(dest);
            
            yield return null;   
        }
    }

    private void MoveLeft()
    {
        Vector3 dir = Vector3.Cross(combatTarget.transform.position - transform.position, combatTarget.transform.up);
        Vector3 dest = transform.position + (dir.normalized * 3.0f);
        // Vector3 dest = transform.position + (-transform.right * 3.0f);
        
        
        // Vector3 dir = Vector3.Cross(transform.position - combatTarget.transform.position, combatTarget.transform.up);

        Debug.DrawRay(transform.position, dir, Color.magenta, 10f);
        
        moveComponent
            .SetLookTarget(combatTarget.transform)
            .SetMoveSpeed(-1.0f)
            .SetDestination(dest);
    }

    private int GetRandomAttackType()
    {
        int type = UnityEngine.Random.Range(1, 4);

        return type;
    }
}
