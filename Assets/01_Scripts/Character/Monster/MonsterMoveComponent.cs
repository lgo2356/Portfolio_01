using UnityEngine;
using UnityEngine.AI;

public class MonsterMoveComponent : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private Vector3 destination;
    private float moveSpeed;

    public Vector3 Destination => destination;
    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        destination = transform.position;
    }

    private void Update()
    {
        animator.SetFloat("MoveSpeedZ", navMeshAgent.velocity.magnitude);
    }

    public void SetDestination(Vector3 dest)
    {
        destination = dest;
        
        if (navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }

        navMeshAgent.SetDestination(dest);
    }

    public MonsterMoveComponent SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
        
        navMeshAgent.speed = speed;

        return this;
    }

    public void StartMove(Vector3 dest, float speed)
    {
        Debug.Assert(speed != 0f);

        destination = dest;
        moveSpeed = speed;

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;

        NavMeshPath navMeshPath = CreateNavMeshPath(dest);
        {
            navMeshAgent.SetPath(navMeshPath);
        }
    }

    public void StopMove()
    {
        destination = transform.position;
        moveSpeed = 0f;
        
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0.0f;

        animator.SetFloat("MoveSpeedZ", 0f);
    }

    private NavMeshPath CreateNavMeshPath(Vector3 dest)
    {
        NavMeshPath navMeshPath = new();

        bool isFoundPath = navMeshAgent.CalculatePath(dest, navMeshPath);
        Debug.Assert(isFoundPath);

        return navMeshPath;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(destination, 0.1f);
    }
}