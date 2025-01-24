using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected Animator animator;
    protected Weapon weapon;
    protected StateComponent stateComponent;

    protected GameObject rootObject;

    protected virtual void Awake()
    {
        Awake_InitRootObject();
        Awake_GetComponents();
    }

    private void Awake_InitRootObject()
    {
        rootObject = transform.root.gameObject;
        Debug.Assert(rootObject != null);
    }

    private void Awake_GetComponents()
    {
        animator = rootObject.GetComponent<Animator>();
        weapon = gameObject.GetComponent<Weapon>();
        stateComponent = rootObject.GetComponent<StateComponent>();
    }

    protected virtual void Start()
    {

    }

    public virtual void DoAction()
    {

    }

    public virtual void BeginAction()
    {

    }

    public virtual void EndAction()
    {

    }

    public virtual void OnCharged()
    {

    }
}
