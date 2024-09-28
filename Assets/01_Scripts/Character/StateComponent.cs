using System;
using UnityEngine;

public class StateComponent : MonoBehaviour
{
    public enum StateType
    {
        Idle = 0,
        Equip, Attack, Guard, Evade, Damaged, Dead,
        Max,
    }

    private StateType curType = StateType.Idle;

    public event Action<StateType, StateType> OnStateTypeChanged;

    public bool IsIdleState { get => curType == StateType.Idle; }
    public bool IsEquipState { get => curType == StateType.Equip; }
    public bool IsAttackState { get => curType == StateType.Attack; }
    public bool IsGuardState { get => curType == StateType.Guard; }
    public bool IsEvadeState { get => curType == StateType.Evade; }
    public bool IsDamagedState { get => curType == StateType.Damaged; }
    public bool IsDeadState { get => curType == StateType.Dead; }

    public void SetIdleState() => ChangeType(StateType.Idle);
    public void SetEquipState() => ChangeType(StateType.Equip);
    public void SetAttackState() => ChangeType(StateType.Attack);
    public void SetGuardState() => ChangeType(StateType.Guard);
    public void SetEvadeState() => ChangeType(StateType.Evade);
    public void SetDamagedState() => ChangeType(StateType.Damaged);
    public void SetDeadState() => ChangeType(StateType.Dead);

    private void ChangeType(StateType newType)
    {
        if (curType == newType)
            return;

        StateType prevType = curType;
        curType = newType;

        OnStateTypeChanged?.Invoke(prevType, newType);
    }
}
