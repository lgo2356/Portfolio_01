using UnityEngine;

public class MeleeWeapon : Weapon
{
    private bool isNextCombo = false;
    private bool isComboEnalbed = false;

    protected override void Awake()
    {
        base.Awake();
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
}
