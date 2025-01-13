using System;
using System.Collections;
using UnityEngine;

public class BossCombatComponent : CombatComponent
{
    [SerializeField, Header("보스")]
    private float backwardDistance = 2.0f;

    [SerializeField]
    private GameObject jumpAttackFX;

    private BossHpComponent hpComponent;

    private bool isCombatPaused = false;
    private bool canSkill = true;
    
    private Coroutine combatCoroutine;

    protected override void Awake()
    {
        base.Awake();

        hpComponent = GetComponent<BossHpComponent>();
    }

    protected override void Start()
    {
        base.Start();
        
        /**
         * 코루틴의 인스턴스는 한번 실행되고 완료되면 종료 상태가 되기 때문에 다시 실행할 수 없다.
         * 코루틴을 다시 실행하려면 새로 인스턴스를 생성해야 한다.
         */
        waitCoroutines = new Func<IEnumerator>[]
        {
            // Coroutine_MoveBackward,
            Coroutine_MoveRight,
            Coroutine_MoveLeft,
        };
    }

    public override void StartCombat(GameObject target)
    {
        base.StartCombat(target);

        combatCoroutine = StartCoroutine(Coroutine_Combat());

        hpComponent.ShowHpBar();
    }

    public override void StopCombat()
    {
        base.StopCombat();

        if (combatCoroutine != null)
        {
            StopCoroutine(combatCoroutine);
            combatCoroutine = null;

            waitTime = 0.0f;
        }
    }

    private IEnumerator Coroutine_Combat()
    {
        while (combatTarget != null)
        {
            if (isCombatPaused == false)
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
                    moveComponent.StopMove();
                    transform.LookAt(combatTarget.transform);

                    if (canAttack)
                    {
                        animator.SetInteger("AttackType", GetRandomAttackType());
                        weaponController.DoAction();

                        float coolTime = GetAttackCoolTime();
                        StartCoroutine(Coroutine_SetCoolTime(coolTime));
                    }

                    if (stateComponent.IsAttackState == false)
                    {
                        waitTime = GetWaitTime(1.5f, 2.5f);

                        if (Vector3.Distance(combatTarget.transform.position, transform.position) < attackDistance - 2.0f)
                        {
                            StartCoroutine(Coroutine_MoveBackward());
                        }
                        else
                        {
                            int waitType = GetWaitCoroutineType();
                            IEnumerator waitCoroutine = waitCoroutines[waitType]();
                            StartCoroutine(waitCoroutine);
                        }

                        yield return new WaitForSeconds(waitTime);
                    }
                }
            }

            if (combatTarget == null)
                break;

            if (Vector3.Distance(combatTarget.transform.position, transform.position) > 9.0f)
            {
                if (canSkill)
                {
                    StartCoroutine(Coroutine_JumpAttack());
                    StartCoroutine(Coroutine_SkillCooltime(15.0f));
                }
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

    private IEnumerator Coroutine_JumpAttack()
    {
        isCombatPaused = true;

        moveComponent.StopMove();

        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        nevMeshAgent.updatePosition = false;

        float jumpDelta = 0.1f;
        float height = 0.0f;
        float originalPosition = transform.position.y;

        yield return new WaitForSeconds(1.0f);

        animator.SetTrigger("DoJump");

        while (height < 5.0f)
        {
            Vector3 position = transform.position;
            position.y += jumpDelta * 2.0f;

            transform.position = position;

            height = transform.position.y - originalPosition;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        float timer = 0.0f;
        Vector3 jumpPosition = combatTarget.transform.position;

        while (Vector3.Distance(transform.position, jumpPosition) > 0.1f)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, jumpPosition, timer / 2.0f);

            if (Vector3.Distance(transform.position, jumpPosition) <= 1f)
            {
                OnLand();
            }

            yield return null;
        }

        nevMeshAgent.updatePosition = true;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;

        isLand = false;
        isCombatPaused = false;
    }
    
    private int GetRandomAttackType()
    {
        int type = UnityEngine.Random.Range(1, 4);

        return type;
    }

    bool isLand = false;
    private void OnLand()
    {
        if (isLand)
        {
            return;
        }

        isLand = true;
        animator.SetTrigger("DoLand");

        GameObject go = Instantiate(jumpAttackFX);
        go.transform.position = transform.position;

        LayerMask layerMask = 1 << LayerMask.NameToLayer("Player");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5.0f, layerMask);
        WeaponController weaponController = GetComponent<WeaponController>();
        Weapon weapon = GetComponent<WeaponController>().CurrentWeapon;

        foreach (Collider collider in colliders)
        {
            //Character character = collider.GetComponent<Character>();
            //Vector3 direction = collider.gameObject.transform.position - transform.position;
            //character.OnKnockdown(direction);

            collider.gameObject.transform.LookAt(transform.position);

            IDamagable damagable = collider.GetComponent<IDamagable>();
            damagable.OnDamaged(gameObject, weapon, Vector3.zero, weapon.WeaponDatas[1]);

            Rigidbody colliderRigidbody = collider.gameObject.GetComponent<Rigidbody>();
            colliderRigidbody.AddExplosionForce(600f, transform.position, 5.0f, 0f, ForceMode.Impulse);
        }
    }

    private IEnumerator Coroutine_SkillCooltime(float time)
    {
        canSkill = false;

        yield return new WaitForSeconds(time);

        canSkill = true;
    }
}
