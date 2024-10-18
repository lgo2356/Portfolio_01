using System;
using System.Collections;
using UnityEngine;

public class ChaseComponent : MonoBehaviour
{
    [SerializeField]
    private float distanceFromTarget = 1.2f;

    private MonsterMoveComponent moveComponent;

    private GameObject target;
    private float chaseSpeed;

    public event Action<GameObject> OnCompleteAction;

    public float ChaseSpeed { set => chaseSpeed = value; }

    private void Awake()
    {
        moveComponent = GetComponent<MonsterMoveComponent>();
    }

    //private void Update()
    //{
    //    if (target == null)
    //        return;

    //    moveComponent.StartMove(target.transform.position, 1.5f);

    //    //if (Vector3.Distance(target.transform.position, transform.position) <= distanceFromTarget)
    //    //{
    //    //    OnCompleteAction?.Invoke(target);

    //    //    StopChase();

    //    //    distanceFromTarget = 2f;
    //    //}
    //    //else
    //    //{
    //    //    moveComponent
    //    //        .SetDestination(target.transform.position)
    //    //        .SetMoveSpeed(1.5f)
    //    //        .StartMove();

    //    //    distanceFromTarget = 1.2f;
    //    //}
    //}

    private IEnumerator Coroutine_Chase(GameObject target, float speed)
    {
        moveComponent.SetMoveSpeed(speed);

        while (true)
        {
            moveComponent.SetDestination(target.transform.position);

            yield return null;
        }
    }

    public void StartChase(GameObject target, float speed)
    {
        this.target = target;
        chaseSpeed = speed;

        StartCoroutine(Coroutine_Chase(target, speed));
    }

    public void StopChase()
    {
        enabled = false;

        moveComponent.StopMove();

        target = null;
    }
}
