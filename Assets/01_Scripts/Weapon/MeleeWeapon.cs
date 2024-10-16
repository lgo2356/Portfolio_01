using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    private bool isInpuNextCombo = false;
    private bool isNextComboEnalbed = false;
    private int comboIndex = 0;

    protected Collider[] colliders;
    private List<GameObject> hitObjectList;

    protected override void Awake()
    {
        base.Awake();

        colliders = GetComponentsInChildren<Collider>();
        hitObjectList = new();
    }

    protected override void Start()
    {
        DisableCollision();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == rootObject)
        {
            return;
        }

        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable == null)
        {
            return;
        }

        if (hitObjectList.Find(hitObj => hitObj == other.gameObject))
        {
            return;
        }

        hitObjectList.Add(other.gameObject);

        damagable.OnDamaged(rootObject, this, Vector3.zero, weaponDatas[comboIndex]);
    }

    public void EnableCombo()
    {
        isNextComboEnalbed = true;
    }

    public void DisableCombo()
    {
        isNextComboEnalbed = false;
    }

    public override void DoNextCombo()
    {
        base.DoNextCombo();

        if (isInpuNextCombo == false)
            return;

        isInpuNextCombo = false;
        comboIndex++;

        animator.SetTrigger("DoNextCombo");
    }

    public override void DoAction()
    {
        if (isNextComboEnalbed)
        {
            isNextComboEnalbed = false;
            isInpuNextCombo = true;

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

        isInpuNextCombo = false;
        isNextComboEnalbed = false;
        comboIndex = 0;
    }

    public virtual void EnableCollision()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    public virtual void EnableCollision(int index)
    {
        colliders[index].enabled = true;
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
