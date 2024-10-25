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
            {
                Vector3 direction = rootObject.transform.forward;

                projectile.Direction = direction;
            }
        }
    }
}
