using System.Collections;
using UnityEngine;

public class MeleeCombatComponent : CombatComponent
{
    [Header("근접")]
    [SerializeField]
    private float combatDistance = 10f;
    
    private Vector3 combatPosition;
    
    private Coroutine combatCoroutine;
    private Coroutine checkCombatRangeCoroutine;
    
    public override void StartCombat(GameObject target)
    {
        base.StartCombat(target);
        
        combatPosition = transform.position;
        
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
        
        combatPosition = Vector3.zero;
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

                animator.SetInteger("ActionType", (int)weaponType);
                weaponController.DoAction();
                
                //TODO : 플레이어 사망 체크하기

                float coolTime = GetAttackCoolTime();

                yield return new WaitForSeconds(coolTime);
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
