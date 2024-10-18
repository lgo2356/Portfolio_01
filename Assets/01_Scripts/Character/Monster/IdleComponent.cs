using System;
using UnityEngine;

public class IdleComponent : MonoBehaviour
{
    private AIStateComponent stateComponent;
    private MonsterMoveComponent moveComponent;
    private PerceptionComponent perceptionComponent;
    private PatrolComponent patrolComponent;
    private CombatComponent combatComponent;

    private void Awake()
    {
        stateComponent = GetComponent<AIStateComponent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
        perceptionComponent = GetComponent<PerceptionComponent>();
        patrolComponent = GetComponent<PatrolComponent>();
        combatComponent = GetComponent<CombatComponent>();
    }

    public void StartIdle()
    {
        moveComponent.StopMove();
        perceptionComponent.ClearPerceivedObject();
        
        stateComponent.SetPatrolState();
    }

    public void StopIdle()
    {
        
    }
}
