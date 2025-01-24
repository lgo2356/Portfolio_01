using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateComponent : MonoBehaviour
{
    public enum StateType
    {
        Idle = 0,
        Equip, Attack, Guard, Evade, Avoid, Damaged, Dead, Knockdown,
        Max,
    }

    public enum SubStateType
    {
        Hold, Toughness
    }

    private Animator animator;
    private WeaponController weaponController;

    private StateType currentState = StateType.Idle;
    private Dictionary<SubStateType, bool> subStateTypes;

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

        subStateTypes = new();
        {
            subStateTypes.Add(SubStateType.Hold, false);
            subStateTypes.Add(SubStateType.Toughness, false);
        }
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

    public bool GetSubType(SubStateType type)
    {
        return subStateTypes[type];
    }

    public void SetSubType(SubStateType type)
    {
        subStateTypes[type] = true;
    }

    public void UnsetSubType(SubStateType type)
    {
        subStateTypes[type] = false;
    }

    private bool isHold;
    private bool isTough;

#if UNITY_EDITOR
    [CustomEditor(typeof(StateComponent))]
    public class StateComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
                return;

            StateComponent script = (StateComponent)target;

            //bool isHold = script.GetSubType(SubStateType.Hold);
            //bool isTough = script.GetSubType(SubStateType.Toughness);

            script.isHold = EditorGUILayout.Toggle("Hold", script.isHold);
            script.isTough = EditorGUILayout.Toggle("Toughness", script.isTough);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
            }

            if (script.isHold)
            {
                script.SetSubType(SubStateType.Hold);
            }
            else
            {
                script.UnsetSubType(SubStateType.Hold);
            }

            if (script.isTough)
            {
                script.SetSubType(SubStateType.Toughness);
            }
            else
            {
                script.UnsetSubType(SubStateType.Toughness);
            }
        }
    }
#endif
}
