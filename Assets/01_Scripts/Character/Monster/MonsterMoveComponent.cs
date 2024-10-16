using UnityEngine;
using UnityEngine.AI;

public class MonsterMoveComponent : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private Vector3 destination;
    private float moveSpeed;
    private GameObject lookTarget;

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

    //TODO : Y 축으로 이동하는 거 고려하기
    //TODO : Distance 계산할 때 Y 값 제외하는 법 생각하기
    private void Update()
    {
        //if (Vector3.Distance(destination, transform.position) < 0.1f)
        //{
        //    animator.SetFloat("MoveSpeedZ", 0f);

        //    StopMove();
        //}
        //else
        //{
        //    animator.SetFloat("MoveSpeedZ", moveSpeed);

        //    Vector3 lookDirection;

        //    if (lookTarget != null)
        //    {
        //        lookDirection = lookTarget.transform.position - transform.position;
        //    }
        //    else
        //    {
        //        lookDirection = destination - transform.position;
        //        lookDirection.y = 0f;
        //    }

        //    Quaternion lookRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
        //    transform.rotation = lookRotation;

        //    transform.position += transform.forward * moveSpeed * Time.deltaTime;
        //}

        animator.SetFloat("MoveSpeedZ", navMeshAgent.velocity.magnitude);
    }

    public MonsterMoveComponent SetDestination(Vector3 dest)
    {
        destination = dest;

        return this;
    }

    public MonsterMoveComponent SetMoveSpeed(float speed)
    {
        moveSpeed = speed;

        return this;
    }

    public MonsterMoveComponent SetLookTarget(GameObject target)
    {
        lookTarget = target;

        return this;
    }

    public void StartMove()
    {
        if (enabled == false)
            enabled = true;

        Debug.Assert(destination != null, "목적지가 설정되지 않았습니다.");
        Debug.Assert(moveSpeed != 0f, "이동 속도가 0 입니다.");

        NavMeshPath navMeshPath = CreateNavMeshPath(destination);
        {
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.SetPath(navMeshPath);
        }
    }

    public void StopMove()
    {
        destination = transform.position;
        moveSpeed = 0f;

        animator.SetFloat("MoveSpeedZ", 0f);

        if (enabled)
            enabled = false;
    }

    private NavMeshPath CreateNavMeshPath(Vector3 destination)
    {
        NavMeshPath navMeshPath = new();

        bool isFoundPath = navMeshAgent.CalculatePath(destination, navMeshPath);
        Debug.Assert(isFoundPath);

        return navMeshPath;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(destination, 0.1f);
    }
}
