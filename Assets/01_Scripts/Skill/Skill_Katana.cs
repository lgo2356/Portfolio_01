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

    public override void BeginAction()
    {
        base.BeginAction();

        stateComponent.SetSubType(StateComponent.SubStateType.Hold);
    }

    public override void EndAction()
    {
        base.EndAction();

        stateComponent.UnsetSubType(StateComponent.SubStateType.Hold);
        stateComponent.UnsetSubType(StateComponent.SubStateType.Toughness);
    }

    public override void OnCharged()
    {
        base.OnCharged();

        stateComponent.SetSubType(StateComponent.SubStateType.Toughness);
    }
}
