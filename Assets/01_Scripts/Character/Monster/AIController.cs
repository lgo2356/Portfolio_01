using System;
using UnityEngine;

[RequireComponent(typeof(PerceptionComponent))]
[RequireComponent(typeof(PatrolComponent))]
[RequireComponent(typeof(CombatComponent))]
public class AIController : MonoBehaviour
{
    public enum AIState
    {
        Idle = 0,
        Patrol, Chase, Combat,
        Max,
    }

    private PerceptionComponent perceptionComponent;
    private PatrolComponent patrolComponent;
    private CombatComponent combatComponent;

    private AIState currentState = AIState.Idle;

    public event Action<AIState, AIState> OnAIStateChanged;

    public bool IsIdleState => currentState == AIState.Idle;
    public bool IsPatrolState => currentState == AIState.Patrol;
    public bool IsChaseState => currentState == AIState.Chase;
    public bool IsCombatState => currentState == AIState.Combat;

    private void Awake()
    {
        perceptionComponent = GetComponent<PerceptionComponent>();
        patrolComponent = GetComponent<PatrolComponent>();
        combatComponent = GetComponent<CombatComponent>();
    }

    private void Start()
    {
        Start_BindEvent();
    }

    private void Start_BindEvent()
    {
        #region Perception
        perceptionComponent.OnFoundAction += (found) =>
        {
            print($"Found : {found.name}");

            patrolComponent.StopPatrol();
            combatComponent.StartCombat(found);
        };

        perceptionComponent.OnLostAction += (lost) =>
        {
            print($"Lost : {lost.name}");

            patrolComponent.StartPatrol();
        };
        #endregion
    }

    public void SetIdle()
    {
        ChangeState(AIState.Idle);
    }

    public void SetPatrol()
    {
        ChangeState(AIState.Patrol);
    }

    public void SetChase()
    {
        ChangeState(AIState.Chase);
    }

    public void SetCombat()
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
