using UnityEngine;

[RequireComponent(typeof(MonsterMoveComponent))]
[RequireComponent(typeof(WeaponController))]
public class CombatComponent : MonoBehaviour
{
    [SerializeField]
    protected float combatDistance = 10f;

    [SerializeField]
    protected float attackDistance = 2.0f;

    [SerializeField]
    protected float attackCoolTime = 2.0f;

    [SerializeField]
    protected float attackCoolTimeDeviation = 0.5f;

    [SerializeField]
    protected WeaponType weaponType;

    protected Animator animator;
    protected AIStateComponent stateComponent;
    protected MonsterMoveComponent moveComponent;
    protected WeaponController weaponController;

    protected GameObject combatTarget;
    protected Vector3 combatPosition;
    protected Collider[] colliderBuffer;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        stateComponent = GetComponent<AIStateComponent>();
        moveComponent = GetComponent<MonsterMoveComponent>();
        weaponController = GetComponent<WeaponController>();
    }

    public virtual void StartCombat(GameObject target)
    {
        print("Start Combat");
        
        combatTarget = target;
        combatPosition = transform.position;
        
        weaponController.SetWeaponType(weaponType);
    }

    public virtual void StopCombat()
    {
        print("Stop Combat");
        
        weaponController.SetWeaponType(weaponType);
    }
    
    protected float GetAttackCoolTime()
    {
        float result = attackCoolTime;
        float deviation = UnityEngine.Random.Range(-attackCoolTimeDeviation, attackCoolTimeDeviation);

        result += deviation;
        
        return result;
    }

    // private IEnumerator Coroutine_Wait()
    // {
    //     moveComponent.SetDestination(transform.position);
    //
    //     while (true)
    //     {
    //         transform.LookAt(combatTarget.transform);
    //
    //         if (Vector3.Distance(moveComponent.Destination, transform.position) < 0.1f)
    //         {
    //             moveComponent.StartMove(GetRandomPosition(), 0.8f);
    //         }
    //
    //         yield return null;
    //     }
    // }
    //
    // private Vector3 GetRandomPosition()
    // {
    //     float x = UnityEngine.Random.Range(-0.5f, 0.5f);
    //     float z = UnityEngine.Random.Range(-0.3f, 0.3f);
    //
    //     Vector3 delta = new(x, 0, z);
    //
    //     return transform.position + delta;
    // }
}
