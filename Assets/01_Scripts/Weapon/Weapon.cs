using Cinemachine;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public float Power;
    public float LaunchDistance;
    public int ImpactIndex;
    public bool IsCanMove;

    public int HitStopFrame;
    public GameObject HitParticle;
    public Vector3 HitParticleScaleOffset = Vector3.zero;
    public Vector3 HitParticlePositionOffset;

    public float CameraShakeDuration;
    public Vector3 CameraShakeDirection;
    public Vector3 CameraShakeDirectionDeviation;
}

public enum WeaponType
{
    Unarmed = 0,
    Fist, Sword, GreatSword, Katana, Staff, Warp,
    Max,
}

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected WeaponType type;

    [SerializeField]
    protected WeaponData[] weaponDatas;

    protected Animator animator;
    protected StateComponent stateComponent;
    protected PlayerMoveComponent moveComponent;
    protected CinemachineImpulseSource impulseSource;

    protected GameObject rootObject;

    public WeaponType Type => type;

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
        
        impulseSource = GetComponent<CinemachineImpulseSource>();
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

    public virtual void ShootProjectile()
    {
        
    }

    public virtual void ShowFX()
    {
        
    }

    protected void SetPlayerMove()
    {
        if (moveComponent == null)
            return;

        moveComponent.Hold();
    }
}
