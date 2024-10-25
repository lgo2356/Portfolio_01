using System;
using UnityEngine;

//TODO : 유도 미사일 추가
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float force = 1000.0f;

    private new Rigidbody rigidbody;
    private new Collider collider;
    private Vector3 direction;

    public event Action<Collider, Collider, Vector3, Vector3> OnProjectileCollision;
    
    public Vector3 Direction
    {
        set => direction = value;
    }
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void Start()
    {
        rigidbody.AddForce(direction * force);
    }
}
