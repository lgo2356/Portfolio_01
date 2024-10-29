using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HpComponent))]
public class Character : MonoBehaviour
{
    protected new Rigidbody rigidbody;
    protected Animator animator;
    protected StateComponent stateComponent;
    protected HpComponent hpComponent;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        stateComponent = GetComponent<StateComponent>();
        hpComponent = GetComponent<HpComponent>();
    }

    protected virtual void Start()
    {

    }

    private void OnAnimatorMove()
    {
        transform.position += animator.deltaPosition;
    }
}
