using UnityEngine;
using WeaponType = WeaponComponent.WeaponType;

[System.Serializable]
public class WeaponData
{
    public float Power;
    public float LaunchDistance;
    public float HitStopFrame;
    public int ImpactIndex;
    public bool IsCanMove;

    public GameObject HitParticle;
    public Vector3 HitParticleScaleOffset = Vector3.one;
    public Vector3 HitParticlePositionOffset;
}

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected WeaponType weaponType;

    [SerializeField]
    protected WeaponData[] weaponDatas;

    protected Animator animator;
    protected StateComponent stateComponent;
    protected PlayerMoveComponent moveComponent;

    protected GameObject rootObject;

    public WeaponType WeaponType => weaponType;

    protected virtual void Reset()
    {

    }

    protected virtual void Awake()
    {
        rootObject = transform.root.gameObject;
        Debug.Assert(rootObject != null);

        Awake_GetComponent();
    }

    private void Awake_GetComponent()
    {
        animator = rootObject.GetComponent<Animator>();
        stateComponent = rootObject.GetComponent<StateComponent>();
        moveComponent = rootObject.GetComponent<PlayerMoveComponent>();
    }

    protected virtual void Start()
    {

    }

    public virtual void Equip()
    {

    }

    public virtual void Unequip()
    {
        
    }

    public virtual void DoNextCombo()
    {
        
    }

    public virtual void DoAction()
    {

    }

    public virtual void EndAction()
    {
        stateComponent.SetIdleState();

        if (moveComponent != null)
        {
            moveComponent.Release();
        }
    }

    protected void SetPlayerMove()
    {
        if (moveComponent == null)
            return;

        moveComponent.Hold();
    }
}
