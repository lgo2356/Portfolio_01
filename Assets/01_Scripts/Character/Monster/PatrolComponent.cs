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
    private bool isPatrol;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
    }

    private IEnumerator Coroutine_Patrol(float time)
    {
        while (isPatrol)
        {
            if (Vector3.Distance(destination, transform.position) <= navMeshAgent.stoppingDistance)
            {
                yield return new WaitForSeconds(time);

                MoveToNextPatrolPoint();
            }

            yield return null;
        }
    }

    private void MoveToNextPatrolPoint()
    {
        destination = patrolPoint.GetNextPatrolPosition();
        patrolPoint.UpdateNextIndex();

        moveComponent.StartMove(destination, 1.2f);
    }

    public void StartPatrol()
    {
        print("StartPatrol");
        
        if (patrolCoroutine != null)
        {
            Debug.LogError("Coroutine_Patrol can't be duplicated");
            return;
        }

        isPatrol = true;

        destination = transform.position;
        patrolCoroutine = Coroutine_Patrol(0.0f);
        StartCoroutine(patrolCoroutine);
    }

    public void StopPatrol()
    {
        print("StopPatrol");
        
        if (patrolCoroutine == null)
            return;

        isPatrol = false;

        StopCoroutine(patrolCoroutine);
        patrolCoroutine = null;

        moveComponent.StopMove();
    }
}
