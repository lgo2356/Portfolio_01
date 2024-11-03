using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // [SerializeField]
    // private float force = 1000.0f;

    private new Rigidbody rigidbody;
    private new Collider collider;
    // protected Vector3 direction;

    public event Action<Collider, Collider, Vector3, Vector3> OnProjectileCollision;
    
    // public Vector3 Direction
    // {
    //     set => direction = value;
    // }
    
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
        print($"Collision - {other.gameObject.name}");
        
        OnProjectileCollision?.Invoke(collider, other, transform.position, other.transform.forward);
        
        Destroy(gameObject);
    }
}
