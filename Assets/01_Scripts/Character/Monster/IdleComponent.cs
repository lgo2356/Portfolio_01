using System;
using UnityEngine;

public class IdleComponent : MonoBehaviour
{
    private AIStateComponent stateComponent;
    private MonsterMoveComponent moveComponent;
    private PerceptionComponent perceptionComponent;

    private Vector3 originalPosition;

    private void Awake()
    {
        stateComponent = GetComponent<AIStateComponent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
        perceptionComponent = GetComponent<PerceptionComponent>();
    }

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void StartIdle()
    {
        print("Start Idle");
        
        moveComponent.StopMove();
        perceptionComponent.ClearPerceivedObject();

        if (GetComponent<PatrolComponent>() == null)
        {
            moveComponent.StartMove(originalPosition, 1.8f);
        }
        else
        {
            stateComponent.SetPatrolState();
        }
    }

    public void StopIdle()
    {
        print("Stop Idle");

        originalPosition = transform.position;
    }
}
