using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private new Collider collider;

    public event Action<Collider, Collider, Vector3, Vector3> OnProjectileCollision;
    
    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public virtual void Shoot(Vector3 direction, float force)
    {
        rigidbody.AddForce(direction * force);
    }

    public virtual void Shoot(GameObject target, float speed)
    {
        
    }

    protected void OnTriggerEnter(Collider other)
    {
        OnProjectileCollision?.Invoke(collider, other, transform.position, other.transform.forward);
    }
}
