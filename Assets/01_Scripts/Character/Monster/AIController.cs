using UnityEngine;

[RequireComponent(typeof(MonsterMoveComponent))]
[RequireComponent(typeof(PerceptionComponent))]
[RequireComponent(typeof(ChaseComponent))]
[RequireComponent(typeof(PatrolComponent))]
public class AIController : MonoBehaviour
{
    private MonsterMoveComponent moveComponent;
    private PerceptionComponent playerScanComponent;
    private ChaseComponent chaseComponent;
    private PatrolComponent patrolComponent;

    private void Awake()
    {
        moveComponent = GetComponent<MonsterMoveComponent>();
        playerScanComponent = GetComponent<PerceptionComponent>();
        chaseComponent = GetComponent<ChaseComponent>();
        patrolComponent = GetComponent<PatrolComponent>();
    }

    private void Start()
    {
        //GameObject destObject = GameObject.Find("Destination");
        //chaseComponent.StartChase(destObject);

        Start_BindEvent();
    }

    private void Start_BindEvent()
    {
        playerScanComponent.OnFoundAction += (found) =>
        {
            print($"Found : {found.name}");

            patrolComponent.StopPatrol();
            chaseComponent.StartChase(found);
        };

        playerScanComponent.OnLostAction += (lost) =>
        {
            print($"Lost : {lost.name}");

            chaseComponent.StopChase();
            patrolComponent.StartPatrol();
        };
    }
}
