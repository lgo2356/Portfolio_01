using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StateComponent))]
[RequireComponent(typeof(HPComponent))]
public class Character : MonoBehaviour
{
    protected new Rigidbody rigidbody;
    protected Animator animator;
    protected StateComponent stateComponent;
    protected HPComponent hpComponent;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        stateComponent = GetComponent<StateComponent>();
        hpComponent = GetComponent<HPComponent>();
    }

    protected virtual void Start()
    {

    }

    private void OnAnimatorMove()
    {
        transform.position += animator.deltaPosition;
    }
}
