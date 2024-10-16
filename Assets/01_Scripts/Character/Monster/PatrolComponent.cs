using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MonsterMoveComponent))]
public class PatrolComponent : MonoBehaviour
{
    [SerializeField]
    private PatrolPoint patrolPoint;

    private NavMeshAgent navMeshAgent;
    private MonsterMoveComponent moveComponent;

    private IEnumerator patrolCoroutine;
    private Vector3 destination;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
    }

    private void Start()
    {
        MoveNextPatrolPoint();
    }

    private IEnumerator Corouine_Patrol(float time)
    {
        while (true)
        {
            if (Vector3.Distance(destination, transform.position) <= navMeshAgent.stoppingDistance)
            {
                yield return new WaitForSeconds(time);

                MoveNextPatrolPoint();
            }

            yield return null;
        }
    }

    private void MoveNextPatrolPoint()
    {
        destination = patrolPoint.GetNextMovePosition();
        patrolPoint.UpdateNextIndex();

        moveComponent
            .SetDestination(destination)
            .SetMoveSpeed(1.2f)
            .StartMove();
    }

    public void StartPatrol()
    {
        if (patrolCoroutine != null)
        {
            Debug.LogError("Coroutine_Patrol 코루틴은 중복 실행할 수 없습니다.");
            return;
        }

        patrolCoroutine = Corouine_Patrol(0.0f);
        StartCoroutine(patrolCoroutine);
    }

    public void StopPatrol()
    {
        if (patrolCoroutine == null)
            return;

        StopCoroutine(patrolCoroutine);
        patrolCoroutine = null;

        moveComponent.StopMove();
    }
}
