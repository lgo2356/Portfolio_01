using UnityEngine;
using AIState = AIStateComponent.AIState;

[RequireComponent(typeof(IdleComponent))]
[RequireComponent(typeof(PerceptionComponent))]
public partial class AIController : MonoBehaviour
{
    private StateComponent stateComponent;
    private AIStateComponent aiStateComponent;
    private IdleComponent idleComponent;
    private PerceptionComponent perceptionComponent;
    private PatrolComponent patrolComponent;
    private CombatComponent combatComponent;

    private void Awake()
    {
        stateComponent = GetComponent<StateComponent>();
        aiStateComponent = GetComponent<AIStateComponent>();
        idleComponent = GetComponent<IdleComponent>();
        perceptionComponent = GetComponent<PerceptionComponent>();
        patrolComponent = GetComponent<PatrolComponent>();
        combatComponent = GetComponent<CombatComponent>();
    }

    private void Start()
    {
        Start_BindEvent();
        Start_InitAIStateCanvas();

        aiStateComponent.SetIdleState();
    }

    private void Start_BindEvent()
    {
        #region State
        aiStateComponent.OnAIStateChanged += (prevState, newState) =>
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
                    if (patrolComponent == null)
                        return;
                    
                    patrolComponent.StartPatrol();
                    
                    break;
                }
                
                case AIState.Dead:
                {
                    perceptionComponent.enabled = false;
                    combatComponent.StopCombat();

                    if (patrolComponent != null)
                    {
                        patrolComponent.StopPatrol();    
                    }
                    
                    idleComponent.StopIdle();
                    
                    break;
                }
            }

            switch (prevState)
            {
                case AIState.Idle:
                {
                    idleComponent.StopIdle();
                    break;
                }

                case AIState.Patrol:
                {
                    if (patrolComponent == null)
                        return;
                    
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
            aiStateComponent.SetCombatState();
        };

        perceptionComponent.OnLostAction += (lost) =>
        {
            print($"Lost : {lost.name}");

            aiStateComponent.SetIdleState();
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
