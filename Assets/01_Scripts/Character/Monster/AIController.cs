using UnityEngine;
using AIState = AIStateComponent.AIState;

[RequireComponent(typeof(PerceptionComponent))]
[RequireComponent(typeof(PatrolComponent))]
[RequireComponent(typeof(CombatComponent))]
public partial class AIController : MonoBehaviour
{
    private AIStateComponent stateComponent;
    private IdleComponent idleComponent;
    private PerceptionComponent perceptionComponent;
    private PatrolComponent patrolComponent;
    private CombatComponent combatComponent;
    private WeaponComponent weaponComponent;

    private void Awake()
    {
        stateComponent = GetComponent<AIStateComponent>();
        idleComponent = GetComponent<IdleComponent>();
        perceptionComponent = GetComponent<PerceptionComponent>();
        patrolComponent = GetComponent<PatrolComponent>();
        combatComponent = GetComponent<CombatComponent>();
    }

    private void Start()
    {
        Start_BindEvent();
        Start_InitAIStateCanvas();

        stateComponent.SetPatrolState();
    }

    private void Start_BindEvent()
    {
        #region State
        stateComponent.OnAIStateChanged += (prevState, newState) =>
        {
            print($"{prevState} -> {newState}");
            
            switch (newState)
            {
                case AIState.Idle:
                {
                    idleComponent.StartIdle();
                    break;
                }

                case AIState.Patrol:
                {
                    patrolComponent.StartPatrol();
                    break;
                }
            }

            switch (prevState)
            {
                case AIState.Idle:
                {
                    break;
                }

                case AIState.Patrol:
                {
                    patrolComponent.StopPatrol();
                    break;
                }

                case AIState.Combat:
                {
                    combatComponent.StopCombat();
                    break;
                }
            }
        };
        #endregion

        #region Perception
        perceptionComponent.OnFoundAction += (found) =>
        {
            print($"Found : {found.name}");

            combatComponent.StartCombat(found);
            stateComponent.SetCombatState();
        };

        perceptionComponent.OnLostAction += (lost) =>
        {
            print($"Lost : {lost.name}");

            stateComponent.SetIdleState();
        };
        #endregion
    }

    private void Update()
    {
        Update_UpdateAIStateText();
    }

    private void LateUpdate()
    {
        LateUpdate_Billboard();
    }
}
