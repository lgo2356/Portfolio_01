using UnityEngine;

public class Staff : RangeWeapon
{
    [SerializeField]
    private string handPositionName = "Hand_Staff";

    [SerializeField]
    private string muzzlePositionName = "Hand_Muzzle";

    [SerializeField]
    private GameObject muzzlePrefab;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private bool isGuided;

    private CombatComponent combatComponent;

    private Transform handPositionTransform;
    private Transform muzzlePositionTransform;
    
    protected override void Reset()
    {
        base.Reset();

        type = WeaponType.Staff;
    }

    protected override void Awake()
    {
        base.Awake();

        combatComponent = rootObject.GetComponent<CombatComponent>();

        handPositionTransform = rootObject.transform.FindChildByName(handPositionName);
        {
            Debug.Assert(handPositionTransform != null, "Staff handPositionTransform is null.");
        
            transform.SetParent(handPositionTransform, false);
        }

        muzzlePositionTransform = rootObject.transform.FindChildByName(muzzlePositionName);
        {
            Debug.Assert(muzzlePositionTransform != null, "Staff muzzlePositionTransform is null.");
        }
    }

    protected override void Start()
    {
        base.Start();
        
        gameObject.SetActive(false);
    }
    
    public override void Equip()
    {
        base.Equip();
        
        gameObject.SetActive(true);
    }

    public override void Unequip()
    {
        base.Unequip();
        
        gameObject.SetActive(false);
    }

    public override void ShootProjectile()
    {
        base.ShootProjectile();
        
        SetPlayerMove();

        {
            if (muzzlePrefab == null)
                return;
            
            Vector3 position = muzzlePositionTransform.position;
            Quaternion rotation = rootObject.transform.rotation;

            Instantiate(muzzlePrefab, position, rotation);   
        }

        {
            if (projectilePrefab == null)
                return;

            Vector3 position = muzzlePositionTransform.position;
            position += rootObject.transform.forward * 0.5f;

            GameObject go = Instantiate(projectilePrefab, position, rootObject.transform.rotation);
            Projectile projectile = go.GetComponent<Projectile>();
            projectile.OnProjectileCollision += OnProjectileCollision;
            
            Vector3 direction = rootObject.transform.forward;

            if (isGuided)
            {
                (projectile as GuidedProjectile).Shoot(combatComponent.CombatTarget, 10.0f);
            }
            else
            {
                projectile.Shoot(direction, 1000.0f);
            }
        }
    }

    private void OnProjectileCollision(Collider projectile, Collider other, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (other.gameObject.CompareTag(rootObject.tag))
            return;

        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.OnDamaged(rootObject, this, hitPoint, weaponDatas[0]);
        }

        // if (weaponDatas[0].HitParticle)
        // {
        //     Instantiate(weaponDatas[0].HitParticle, hitPoint, rootObject.transform.rotation);
        // }
    }
}
