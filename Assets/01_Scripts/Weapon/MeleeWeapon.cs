using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    private bool isNextCombo = false;
    private bool isComboEnalbed = false;

    protected Collider[] colliders;
    private List<GameObject> hitObjectList;

    protected override void Awake()
    {
        base.Awake();

        colliders = GetComponentsInChildren<Collider>();
        hitObjectList = new();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == rootObject)
        {
            return;
        }


    }

    public void EnableCombo()
    {
        isComboEnalbed = true;
    }

    public void DisableCombo()
    {
        isComboEnalbed = false;
    }

    public override void DoNextCombo()
    {
        base.DoNextCombo();

        if (isNextCombo == false)
            return;

        isNextCombo = false;

        animator.SetTrigger("DoNextCombo");
    }

    public override void DoAction()
    {
        if (isComboEnalbed)
        {
            isComboEnalbed = false;
            isNextCombo = true;

            return;
        }

        if (stateComponent.IsIdleState == false)
            return;

        base.DoAction();

        SetPlayerMove();
    }

    public override void EndAction()
    {
        base.EndAction();

        isNextCombo = false;
        isComboEnalbed = false;
    }

    public virtual void EnableCollision()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    public virtual void DisableCollision()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        hitObjectList.Clear();
    }
}
