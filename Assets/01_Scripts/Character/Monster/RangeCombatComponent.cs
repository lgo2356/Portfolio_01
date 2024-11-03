using System.Collections;
using UnityEngine;

public class RangeCombatComponent : CombatComponent
{
    [Header("원거리")]
    [SerializeField]
    private float avoidDistance = 5.0f;

    private Coroutine combatCoroutine;
    private Coroutine avoidCoroutine;

    private bool isCombat = true;
    private bool canAttack = true;
    private bool isAvoid = true;

    public override void StartCombat(GameObject target)
    {
        base.StartCombat(target);
        
        Debug.Assert(combatCoroutine == null, "Combat Coroutine은 중복 실행할 수 없습니다.");
        isCombat = true;
        combatCoroutine = StartCoroutine(Coroutine_Attack());
        
        Debug.Assert(avoidCoroutine == null, "Avoid Coroutine은 중복 실행할 수 없습니다.");
        isAvoid = true;
        avoidCoroutine = StartCoroutine(Coroutine_Avoid());
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

    private IEnumerator Coroutine_SetCoolTime(float time)
    {
        canAttack = false;
        
        yield return new WaitForSeconds(time);

        canAttack = true;
    }

    private IEnumerator Coroutine_Avoid()
    {
        while (isAvoid)
        {
            if (Vector3.Distance(combatTarget.transform.position, transform.position) <= avoidDistance)
            {
                print("Avoid!");
            }
            else
            {
                
            }
            
            yield return null;
        }
    }
}
