using System.Collections;
using UnityEngine;

public class RangeCombatComponent : CombatComponent
{
    [Header("원거리")]
    [SerializeField]
    private float avoidRange = 5.0f;

    private Coroutine combatCoroutine;
    private Coroutine avoidCoroutine;

    private bool isCombat = true;

    public override void StartCombat(GameObject target)
    {
        base.StartCombat(target);
        
        Debug.Assert(combatCoroutine == null, "Combat Coroutine은 중복 실행할 수 없습니다.");

        isCombat = true;
        combatCoroutine = StartCoroutine(Coroutine_Attack());
    }

    public override void StopCombat()
    {
        base.StopCombat();

        Debug.Assert(combatCoroutine != null, "Combat Coroutine은 이미 실행 해제되었습니다.");

        StopCoroutine(combatCoroutine);
        combatCoroutine = null;
        isCombat = false;
    }

    private IEnumerator Coroutine_Attack()
    {
        while (isCombat)
        {
            transform.LookAt(combatTarget.transform);
            
            if (Vector3.Distance(combatTarget.transform.position, transform.position) > attackDistance)
            {
                //TODO : 전투 대기
            }
            else
            {
                moveComponent.StopMove();

                weaponController.DoAction();
                
                //TODO : 플레이어 사망 체크하기

                float coolTime = GetAttackCoolTime();

                yield return new WaitForSeconds(coolTime);
            }
            
            yield return null;
        }
    }
}
