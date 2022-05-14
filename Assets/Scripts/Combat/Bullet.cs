using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [HideInInspector] public BulletData_SO bulletData;
    private Rigidbody rb;
    public float bulletSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletSpeed = 1f;
    }

    private void Start()
    {
        rb.AddForce(bulletSpeed * transform.up, ForceMode.Impulse);
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("hit " + other.gameObject.name + "!");
    }
}
