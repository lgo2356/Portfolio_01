using System.Collections;
using UnityEngine;

public class Skill_Staff_Effect : MonoBehaviour
{
    [SerializeField]
    private float radius;

    [SerializeField]
    private float range;

    private LayerMask layerMask;

    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Player");

        StartCoroutine(Coroutine_Hit());
    }

    private IEnumerator Coroutine_Hit()
    {
        WaitForSeconds wait = new WaitForSeconds(0.25f);

        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            var hits = Physics.SphereCastAll(transform.position, radius, Vector3.up, range, layerMask);

            foreach (var hit in hits)
            {
                GameObject go = hit.collider.gameObject;
                IDamagable damagable = go.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    Debug.Log(go.name);

                    damagable.OnDamaged(gameObject, 5.0f);
                }
            }

            yield return wait;
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
