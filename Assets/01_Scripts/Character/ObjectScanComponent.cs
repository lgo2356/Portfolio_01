using System;
using UnityEditor;
using UnityEngine;

public class ObjectScanComponent : MonoBehaviour
{
    [SerializeField]
    private float scanRange;

    [SerializeField]
    private LayerMask layerMask;

    private GameObject foundObject;
    private Collider[] colliders;

    public event Action<GameObject> OnFoundAction;
    public event Action<GameObject> OnLostAction;

    public GameObject FoundObject => foundObject;

    private void Awake()
    {
        colliders = new Collider[1];
    }

    private void Update()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, scanRange, colliders, layerMask);

        if (foundObject != null && count == 0)
        {
            OnLostAction?.Invoke(foundObject);

            foundObject = null;

            return;
        }

        for (int i = 0; i < count; i++)
        {
            Collider collider = colliders[i];

            Debug.DrawRay(transform.position, collider.transform.position - transform.position, Color.red);

            if (foundObject == null)
            {
                foundObject = collider.gameObject;

                OnFoundAction?.Invoke(collider.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, scanRange);
    }
}
