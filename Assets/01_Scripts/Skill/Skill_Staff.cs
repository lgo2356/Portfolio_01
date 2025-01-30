using System.Collections.Generic;
using UnityEngine;

public class Skill_Staff : Skill, ISkillPressedDownHandler, ISkillPressedUpHandler, ISkillPerformedShortHandler
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

    public void OnPerformedShort()
    {
        GameObject target = FindClosestTarget();

        if (target == null)
            return;

        cursor.transform.position = target.transform.position;
        cursor.transform.position += Vector3.up * 0.05f;
        cursor.HoldCursor(true);

        animator.SetBool("IsSkillAction", true);
        animator.SetBool("IsSkillReady", false);
    }

    private GameObject FindClosestTarget()
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Monster");
        Collider[] colliders = Physics.OverlapSphere(rootObject.transform.position, 10.0f, layerMask);
        List<GameObject> candidates = new List<GameObject>();

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Monster>(out var monster))
            {
                candidates.Add(monster.gameObject);
            }
        }

        GameObject result = null;
        float maxAngle = float.MinValue;

        foreach (var candidate in candidates)
        {
            Vector3 direction = candidate.transform.position - rootObject.transform.position;
            float angle = Vector3.Dot(rootObject.transform.forward, direction.normalized);

            if (angle < 0.5f)
                continue;

            if (angle > maxAngle)
            {
                maxAngle = angle;
                result = candidate;
            }
        }

        return result;
    }

    public override void DoAction()
    {
        base.DoAction();

        GameObject effectObject = Instantiate(effectPrefab);
        effectObject.transform.position = cursor.transform.position;
        Skill_Staff_Effect effect = effectObject.GetComponent<Skill_Staff_Effect>();
        effect.RootObject = rootObject;
        
        cursor.SetDisabled();
    }

    public override void BeginAction()
    {
        base.BeginAction();

        cursor.HoldCursor(true);

        stateComponent.SetSubType(StateComponent.SubStateType.Hold);
    }

    public override void EndAction()
    {
        base.EndAction();

        stateComponent.UnsetSubType(StateComponent.SubStateType.Hold);
    }
}
