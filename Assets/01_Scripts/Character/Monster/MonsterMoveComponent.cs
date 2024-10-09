using UnityEngine;

public class MonsterMoveComponent : MonoBehaviour
{
    private Animator animator;

    private Vector3 destination;
    private float moveSpeed;
    private GameObject lookTarget;

    public Vector3 Destination => destination;
    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        destination = transform.position;
    }

    //TODO : Y 축으로 이동하는 거 고려하기
    //TODO : Distance 계산할 때 Y 값 제외하는 법 생각하기
    private void Update()
    {
        //Debug.Log(Vector3.Distance(destination, transform.position));

        if (Vector3.Distance(destination, transform.position) < 0.1f)
        {
            animator.SetFloat("MoveSpeedZ", 0f);

            StopMove();
        }
        else
        {
            animator.SetFloat("MoveSpeedZ", moveSpeed);

            Vector3 lookDirection;

            if (lookTarget != null)
            {
                lookDirection = lookTarget.transform.position - transform.position;
            }
            else
            {
                lookDirection = destination - transform.position;
                lookDirection.y = 0f;
            }

            Quaternion lookRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
            transform.rotation = lookRotation;

            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            Debug.DrawRay(transform.position, lookDirection * 10f, Color.red);
        }
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
    }

    public void StopMove()
    {
        destination = transform.position;
        moveSpeed = 0f;

        animator.SetFloat("MoveSpeedZ", 0f);

        if (enabled)
            enabled = false;
    }
}
