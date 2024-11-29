using UnityEngine;

public class IdleComponent : MonoBehaviour
{
    private AIStateComponent stateComponent;
    private MonsterMoveComponent moveComponent;
    private PerceptionComponent perceptionComponent;
    private PatrolComponent patrolComponent;

    private Vector3 originalPosition;

    private void Awake()
    {
        stateComponent = GetComponent<AIStateComponent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
        perceptionComponent = GetComponent<PerceptionComponent>();
        patrolComponent = GetComponent<PatrolComponent>();
    }

    private void Start()
    {
        originalPosition = transform.position;
        
        StartIdle();
    }

    public void StartIdle()
    {
        print("Start Idle");
        
        moveComponent.StopMove();
        perceptionComponent.ClearPerceivedObject();

        if (patrolComponent == null)
        {
            moveComponent.StartMove(originalPosition, 1.8f);
        }
        else
        {
            if (patrolComponent.CanPatrol)
            {
                stateComponent.SetPatrolState();
            }
            else
            {
                moveComponent.StartMove(originalPosition, 1.8f);
            }
        }
    }

    public void StopIdle()
    {
        print("Stop Idle");

        originalPosition = transform.position;
    }
}
