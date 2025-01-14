using UnityEngine;

public class Skill_Katana : Skill, ISkillPerformedShortHandler, ISkillPerformedLongHandler, ISkillPressedDownHandler, ISkillPressedUpHandler
{
    public void OnPerformedShort()
    {
        animator.SetBool("IsCharging", false);
        animator.SetBool("IsSkillAction", true);
    }

    public void OnPerformedLong()
    {
        //animator.SetBool("IsCharged", true);
    }

    public void OnPressedDown()
    {
        animator.SetBool("IsCharging", true);
        animator.SetBool("IsSkillAction", true);
    }

    public void OnPressedUp()
    {
        animator.SetBool("IsCharging", false);

        if (animator.GetBool("IsCharged") == false)
        {
            animator.SetBool("IsSkillAction", false);
            animator.SetTrigger("DoNextCombo");
        }
    }
}
