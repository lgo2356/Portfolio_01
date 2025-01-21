using UnityEngine;

public class Skill_Staff : Skill, ISkillPressedDownHandler, ISkillPressedUpHandler
{
    [SerializeField]
    private GameObject cursorPrefab;

    [SerializeField]
    private GameObject effectPrefab;

    private Skill_Staff_Cursor cursor;

    protected override void Start()
    {
        base.Start();

        Debug.Assert(cursorPrefab != null, "Skill_Staff cursorPrefab is null.");

        GameObject cursorObject = Instantiate(cursorPrefab);
        cursor = cursorObject.GetComponent<Skill_Staff_Cursor>();
        cursor.gameObject.SetActive(false);
    }

    public void OnPressedDown()
    {
        animator.SetBool("IsSkillReady", true);

        cursor.SetEnabled();
    }

    public void OnPressedUp()
    {
        animator.SetBool("IsSkillAction", true);
        animator.SetBool("IsSkillReady", false);
    }

    public override void DoAction()
    {
        base.DoAction();

        GameObject effectObject = Instantiate(effectPrefab);
        effectObject.transform.position = cursor.transform.position;
        
        cursor.SetDisabled();
    }

    public override void BeginAction()
    {
        base.BeginAction();

        cursor.HoldCursor(true);
    }
}
