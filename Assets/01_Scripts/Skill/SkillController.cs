using UnityEngine;

public class SkillController : MonoBehaviour
{
    private Animator animator;
    private WeaponController weaponController;

    private Skill currentSkill;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        weaponController = GetComponent<WeaponController>();
        {
            weaponController.OnWeaponChanged += OnWeaponChanged;
        }
    }

    private void OnWeaponChanged(Weapon weapon)
    {
        currentSkill = weapon.GetComponent<Skill>();
    }

    public void OnPressedDown()
    {
        ISkillPressedDownHandler handler = currentSkill as ISkillPressedDownHandler;
        handler?.OnPressedDown();
    }

    public void OnPressedUp()
    {
        ISkillPressedUpHandler handler = currentSkill as ISkillPressedUpHandler;
        handler?.OnPressedUp();
    }

    public void OnPerformedShort()
    {
        ISkillPerformedShortHandler handler = currentSkill as ISkillPerformedShortHandler;
        handler?.OnPerformedShort();
    }

    public void OnPerformedLong()
    {
        ISkillPerformedLongHandler handler = currentSkill as ISkillPerformedLongHandler;
        handler?.OnPerformedLong();
    }

    public void EndAction()
    {
        animator.SetBool("IsSkillAction", false);
        animator.SetBool("IsCharging", false);
        animator.SetBool("IsCharged", false);

        currentSkill.EndAction();
    }

    private void Anim_DoAction()
    {
        currentSkill.DoAction();
    }

    private void Anim_BeginAction()
    {
        currentSkill.BeginAction();
    }

    private void Anim_EndAction()
    {
        EndAction();
    }

    private void Anim_EndCharging()
    {
        animator.SetBool("IsCharged", true);

        currentSkill.OnCharged();
    }
}
