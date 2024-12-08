using System;
using UnityEngine;

public class StateComponent : MonoBehaviour
{
    public enum StateType
    {
        Idle = 0,
        Equip, Attack, Guard, Evade, Avoid, Damaged, Dead, Knockdown,
        Max,
    }

    private Animator animator;
    private WeaponController weaponController;

    private StateType currentState = StateType.Idle;

    public event Action<StateType, StateType> OnStateTypeChanged;
    public event Action OnIdleAction;
    public event Action OnDeadAction;
    public event Action OnDamagedAction;
    public event Action OnKnockdownAction;

    public StateType CurrentState => currentState;
    public bool IsIdleState => currentState == StateType.Idle;
    public bool IsEquipState => currentState == StateType.Equip;
    public bool IsAttackState => currentState == StateType.Attack;
    public bool IsGuardState => currentState == StateType.Guard;
    public bool IsEvadeState => currentState == StateType.Evade;
    public bool IsAvoidState => currentState == StateType.Avoid;
    public bool IsDamagedState => currentState == StateType.Damaged;
    public bool IsDeadState => currentState == StateType.Dead;

    public void SetEquipState() => ChangeType(StateType.Equip);
    public void SetAttackState() => ChangeType(StateType.Attack);
    public void SetGuardState() => ChangeType(StateType.Guard);
    public void SetEvadeState() => ChangeType(StateType.Evade);

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponController = GetComponent<WeaponController>();
    }

    public void SetIdleState()
    {
        ChangeType(StateType.Idle);

        OnIdleAction?.Invoke();
    }

    public void SetAvoidState()
    {
        animator.Play($"Blend {weaponController.currentType}", 0);

        if (animator.GetBool("IsAction"))
        {
            weaponController.EndAction();
        }
        
        ChangeType(StateType.Avoid);
    }

    public void SetDamagedState()
    {
        ChangeType(StateType.Damaged);

        OnDamagedAction?.Invoke();
    }

    public void SetDeadState()
    {
        ChangeType(StateType.Dead);
        
        OnDeadAction?.Invoke();
    }

    public void SetKnockdownState()
    {
        ChangeType(StateType.Knockdown);

        OnKnockdownAction?.Invoke();
    }

    private void ChangeType(StateType newType)
    {
        if (currentState == newType)
            return;

        StateType prevType = currentState;
        currentState = newType;

        OnStateTypeChanged?.Invoke(prevType, newType);
    }
}
