using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMoveComponent : MonoBehaviour
{
    public enum Direction
    {
        Forward, Backward, Right, Left,
    }
    
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private Vector3 destination;
    private float moveSpeed;
    private Transform lookTarget;
    private Direction direction;

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
        if (lookTarget != null)
        {
            transform.LookAt(lookTarget);
        }
        
        float velocity = navMeshAgent.velocity.magnitude;
        
        if (moveSpeed < 0.0f)
        {
            velocity *= (-1.0f);
        }

        switch (direction)
        {
            case Direction.Right:
            case Direction.Left:
            {
                animator.SetFloat("MoveSpeedX", velocity);
                break;
            }
            
            case Direction.Forward:
            case Direction.Backward:
            {
                animator.SetFloat("MoveSpeedZ", velocity);
                break;
            }
        }
    }

    public MonsterMoveComponent SetLookTarget(Transform target)
    {
        lookTarget = target;

        return this;
    }

    public MonsterMoveComponent SetDirection(Direction direction)
    {
        this.direction = direction;
        
        return this;
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
        
        navMeshAgent.speed = Mathf.Abs(speed);

        return this;
    }

    public void StartMove(Vector3 dest, float speed)
    {
        Debug.Assert(speed != 0f);

        destination = dest;
        moveSpeed = speed;

        navMeshAgent.speed = Mathf.Abs(speed);
        navMeshAgent.isStopped = false;

        NavMeshPath navMeshPath = CreateNavMeshPath(dest);
        {
            if (navMeshPath != null)
            {
                navMeshAgent.SetPath(navMeshPath);
            }
            else
            {
                Debug.LogError("NavMeshPath Creating has been failed.");
                return;
            }
        }
    }

    public void StopMove()
    {
        destination = transform.position;
        SetDestination(destination);
        
        moveSpeed = 0f;
        
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0.0f;

        animator.SetFloat("MoveSpeedZ", 0f);

        lookTarget = null;
    }

    private NavMeshPath CreateNavMeshPath(Vector3 dest)
    {
        NavMeshPath navMeshPath = new();

        bool isFoundPath = navMeshAgent.CalculatePath(dest, navMeshPath);

        if (isFoundPath == false)
        {
            navMeshPath = null;
            Debug.Assert(isFoundPath, $"{dest} is invalid path.");
        }

        return navMeshPath;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(destination, 0.1f);
    }

    private void OnGUI()
    {
        GUI.color = Color.green;
        GUILayout.Label($"{navMeshAgent.velocity}");
    }
}
