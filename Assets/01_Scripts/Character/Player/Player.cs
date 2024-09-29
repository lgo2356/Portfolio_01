using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform bodyTransform;

    private void Reset()
    {
        GameObject go = GameObject.Find("Player");
        Debug.Assert(go != null);

        bodyTransform = go.transform.FindChildByName("Body");
        Debug.Assert(bodyTransform != null);
    }

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {        
        transform.position += animator.deltaPosition;
    }
}
