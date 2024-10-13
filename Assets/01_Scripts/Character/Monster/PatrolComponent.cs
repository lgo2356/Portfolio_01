using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatrolComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject patrolPath;

    private MonsterMoveComponent moveComponent;

    private Queue<Vector3> forwardPaths;
    private Stack<Vector3> reversePaths;
    private Vector3 currentPatrolPoint;

    private void Awake()
    {
        moveComponent = GetComponent<MonsterMoveComponent>();

        forwardPaths = new();
        reversePaths = new();
    }

    private void Start()
    {
        Start_GetPatrolPath();
    }

    private void Start_GetPatrolPath()
    {
        if (patrolPath == null)
            return;

        Transform start = patrolPath.transform.FindChildByName("StartPoint");
        Transform end = patrolPath.transform.FindChildByName("EndPoint");

        List<Transform> foundPaths = patrolPath.transform.FindChildrenByName("Waypoint", true);
        {
            foundPaths.Insert(0, start);
            foundPaths.Add(end);
        }

        List<Vector3> temp = foundPaths.Select(path => path.position).ToList();

        foreach (Vector3 point in temp)
        {
            forwardPaths.Enqueue(point);
        }

        currentPatrolPoint = forwardPaths.Peek();
    }

    private void Update()
    {
        if (Vector3.Distance(currentPatrolPoint, transform.position) < 0.1f)
        {
            if (forwardPaths.Count > 0)
            {
                Vector3 point = forwardPaths.Dequeue();
                reversePaths.Push(point);

                currentPatrolPoint = point;
            }
            else
            {
                while (reversePaths.Count > 0)
                {
                    Vector3 point = reversePaths.Pop();
                    forwardPaths.Enqueue(point);
                }

                reversePaths.Clear();
            }
        }
        else
        {
            moveComponent
                .SetMoveSpeed(1.2f)
                .SetDestination(currentPatrolPoint)
                .StartMove();
        }
    }

    public void StartPatrol()
    {
        enabled = true;
    }

    public void StopPatrol()
    {
        enabled = false;
    }
}
