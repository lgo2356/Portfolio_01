using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    [SerializeField]
    private float perceptionDistance = 5.0f;

    [SerializeField]
    private float perceptionAngle = 45.0f;

    [SerializeField]
    private float lostTime = 2.0f;

    [SerializeField]
    private LayerMask layerMask;

    private void Reset()
    {
        layerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private Collider[] colliderBuffer;
    private Dictionary<GameObject, float> perceivedTable;

    public event Action<GameObject> OnFoundAction;
    public event Action<GameObject> OnLostAction;

    private void Awake()
    {
        colliderBuffer = new Collider[10];
        perceivedTable = new();
    }

    private void Start()
    {
        StartCoroutine(Coroutine_RemoveOutOfPerceivedTime());
    }

    private void Update()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, perceptionDistance, colliderBuffer, layerMask);
        List<Collider> candidates = new();

        for (int i = 0; i < count; i++)
        {
            Vector3 direction = colliderBuffer[i].transform.position - transform.position;
            float signedAngle = Vector3.SignedAngle(transform.forward, direction.normalized, Vector3.up);

            if (Mathf.Abs(signedAngle) < perceptionAngle / 2f)
            {
                candidates.Add(colliderBuffer[i]);

                Debug.DrawRay(transform.position, direction, Color.blue, 0.1f);
            }
        }

        foreach (Collider candidate in candidates)
        {
            if (perceivedTable.ContainsKey(candidate.gameObject))
            {
                perceivedTable[candidate.gameObject] = Time.realtimeSinceStartup;
            }
            else
            {
                perceivedTable.Add(candidate.gameObject, Time.realtimeSinceStartup);

                OnFoundAction?.Invoke(candidate.gameObject);
            }
        }
    }

    private IEnumerator Coroutine_RemoveOutOfPerceivedTime()
    {
        List<GameObject> removeReservationList = new();

        while (true)
        {
            foreach (var perceivedData in perceivedTable)
            {
                if ((Time.realtimeSinceStartup - perceivedData.Value) >= lostTime)
                {
                    OnLostAction?.Invoke(perceivedData.Key);

                    removeReservationList.Add(perceivedData.Key);
                }
            }

            removeReservationList.RemoveAll((go) =>
            {
                return perceivedTable.Remove(go);
            });

            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, perceptionDistance);

        Gizmos.color = Color.blue;

        Vector3 direction;

        direction = Quaternion.AngleAxis(+perceptionAngle / 2f, Vector3.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + direction.normalized * perceptionDistance);

        direction = Quaternion.AngleAxis(-perceptionAngle / 2f, Vector3.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + direction.normalized * perceptionDistance);
    }
#endif
}
