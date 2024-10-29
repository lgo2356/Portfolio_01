using System.Collections;
using UnityEngine;

public class MeleeCombatComponent : CombatComponent
{
    private Coroutine combatCoroutine;
    private Coroutine checkCombatRangeCoroutine;
    
    public override void StartCombat(GameObject target)
    {
        base.StartCombat(target);
        
        checkCombatRangeCoroutine = StartCoroutine(Coroutine_CheckCombatRange());
        combatCoroutine = StartCoroutine(Coroutine_Attack());
    }

    public override void StopCombat()
    {
        base.StopCombat();
        
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
}
