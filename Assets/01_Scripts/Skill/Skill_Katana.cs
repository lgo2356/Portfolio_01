using UnityEngine;

public class Skill_Katana : Skill, ISkillPerformedShortHandler, ISkillPressedDownHandler, ISkillPressedUpHandler
{
    public void OnPerformedShort()
    {
        animator.SetBool("IsCharging", false);
    }

    public void OnPressedDown()
    {
        animator.SetBool("IsCharging", true);
        animator.SetBool("IsSkillAction", true);
    }

    public void OnPressedUp()
    {
        animator.SetBool("IsCharging", false);
        animator.SetBool("IsSkillAction", false);
    }
}
