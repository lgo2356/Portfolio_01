using System;
using UnityEngine;

public class AIStateComponent : MonoBehaviour
{
    public enum AIState
    {
        Idle = 0,
        Patrol, Chase, Combat,
        Max,
    }

    private AIState currentState = AIState.Idle;

    public event Action<AIState, AIState> OnAIStateChanged;

    public AIState CurrentState => currentState;
    public bool IsIdleState => currentState == AIState.Idle;
    public bool IsPatrolState => currentState == AIState.Patrol;
    public bool IsChaseState => currentState == AIState.Chase;
    public bool IsCombatState => currentState == AIState.Combat;

    public void SetIdleState()
    {
        ChangeState(AIState.Idle);
    }

    public void SetPatrolState()
    {
        ChangeState(AIState.Patrol);
    }

    public void SetChaseState()
    {
        ChangeState(AIState.Chase);
    }

    public void SetCombatState()
    {
        ChangeState(AIState.Combat);
    }

    public void ChangeState(AIState newState)
    {
        if (currentState == newState)
            return;

        AIState prevState = currentState;
        currentState = newState;

        OnAIStateChanged?.Invoke(prevState, newState);
    }
}
