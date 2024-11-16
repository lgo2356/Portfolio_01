using System;
using UnityEngine;

public class PortalEventTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject portalObject;

    [SerializeField]
    private GameObject bossObject;

    private new Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void Start()
    {
        collider.enabled = false;
        
        StateComponent bossState = bossObject.GetComponent<StateComponent>();
        bossState.OnDeadAction += () =>
        {
            collider.enabled = true;
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        portalObject.SetActive(true);
    }
}
