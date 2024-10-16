using System;
using UnityEngine;

public class ChaseComponent : MonoBehaviour
{
    [SerializeField]
    private float distanceFromTarget = 1.2f;

    private MonsterMoveComponent moveComponent;

    private GameObject target;

    public event Action<GameObject> OnCompleteAction;

    private void Awake()
    {
        moveComponent = GetComponent<MonsterMoveComponent>();
    }

    private void Update()
    {
        if (target == null)
            return;

        moveComponent
            .SetDestination(target.transform.position)
            .SetMoveSpeed(1.5f)
            .StartMove();

        //if (Vector3.Distance(target.transform.position, transform.position) <= distanceFromTarget)
        //{
        //    OnCompleteAction?.Invoke(target);

        //    StopChase();

        //    distanceFromTarget = 2f;
        //}
        //else
        //{
        //    moveComponent
        //        .SetDestination(target.transform.position)
        //        .SetMoveSpeed(1.5f)
        //        .StartMove();

        //    distanceFromTarget = 1.2f;
        //}
    }

    public void StartChase(GameObject target)
    {
        this.target = target;

        moveComponent.SetMoveSpeed(2f);

        enabled = true;
    }

    public void StopChase()
    {
        enabled = false;

        moveComponent.StopMove();

        target = null;
    }
}
