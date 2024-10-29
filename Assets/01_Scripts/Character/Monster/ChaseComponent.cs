using System.Collections;
using UnityEngine;

public class ChaseComponent : MonoBehaviour
{
    [SerializeField]
    private float distanceFromTarget = 1.2f;

    private MonsterMoveComponent moveComponent;

    private GameObject target;
    private float chaseSpeed;

    public float ChaseSpeed { set => chaseSpeed = value; }

    private void Awake()
    {
        moveComponent = GetComponent<MonsterMoveComponent>();
    }

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
