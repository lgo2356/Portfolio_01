using System.Collections.Generic;
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
    private GameObject hitPrefab;

    [SerializeField]
    private bool isGuided;

    [SerializeField]
    private float aimAngle = 15.0f;

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
            
            Vector3 direction;
            GameObject target = GetGuidedTarget();

            if (target == null)
            {
                direction = rootObject.transform.forward;
            }
            else
            {
                direction = target.transform.position - rootObject.transform.position;
            }

            if (isGuided)
            {
                (projectile as GuidedProjectile).Shoot(combatComponent.CombatTarget, 10.0f);
            }
            else
            {
                projectile.Shoot(direction.normalized, 1000.0f);
            }
        }
    }

    private void OnProjectileCollision(Collider projectileCollider, Collider otherCollider, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (otherCollider.gameObject.CompareTag(rootObject.tag))
            return;

        print($"Projectile Collision - {otherCollider.gameObject.name}");

        GameObject hitObject = Instantiate(hitPrefab);
        hitObject.transform.position = projectileCollider.gameObject.transform.position;

        Destroy(projectileCollider.gameObject);

        IDamagable damagable = otherCollider.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.OnDamaged(rootObject, this, hitPoint, weaponDatas[0]);
        }
    }

    private GameObject GetGuidedTarget()
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Monster");
        Vector3 position = rootObject.transform.position;
        float radius = 12.0f;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        List<GameObject> candidates = new();

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<Monster>(out var monster))
            {
                candidates.Add(monster.gameObject);
            }
        }

        GameObject result = null;
        float maxAngle = float.MinValue;

        foreach (GameObject candidate in candidates)
        {
            Vector3 direction = candidate.transform.position - position;
            float angle = Vector3.SignedAngle(rootObject.transform.forward, direction.normalized, Vector3.up);

            if (Mathf.Abs(angle) > aimAngle)
                continue;

            if (angle > maxAngle)
            {
                maxAngle = angle;
                result = candidate;

                Debug.DrawLine(position, position + (result.transform.position - position), Color.blue, 5.0f);
            }
        }

        //Vector3 d = Quaternion.AngleAxis(+45.0f, Vector3.up) * rootObject.transform.forward;
        //Debug.DrawLine(position, position + d.normalized * radius, Color.red, 5.0f);

        //d = Quaternion.AngleAxis(-45.0f, Vector3.up) * rootObject.transform.forward;
        //Debug.DrawLine(position, position + d.normalized * radius, Color.red, 5.0f);

        return result;
    }
}
