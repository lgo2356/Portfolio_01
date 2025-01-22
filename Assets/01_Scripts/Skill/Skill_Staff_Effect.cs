using System.Collections;
using UnityEngine;

public class Skill_Staff_Effect : MonoBehaviour
{
    [SerializeField]
    private float radius;

    [SerializeField]
    private float range;

    [SerializeField]
    private WeaponData weaponData;

    private LayerMask layerMask;

    public GameObject RootObject { get; set; }

    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Monster");

        StartCoroutine(Coroutine_Hit());
    }

    private IEnumerator Coroutine_Hit()
    {
        WaitForSeconds wait = new(0.25f);

        yield return new WaitForSeconds(1.0f);

        float timer = 0.0f;

        while (timer < 2.0f)
        {
            var hits = Physics.SphereCastAll(transform.position, radius, Vector3.up, range, layerMask);

            foreach (var hit in hits)
            {
                GameObject go = hit.collider.gameObject;
                IDamagable damagable = go.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    damagable.OnDamaged(RootObject, 2, weaponData);
                }
            }

            yield return wait;
            timer += 0.25f;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position + transform.up * range, radius);
    }
#endif
}
