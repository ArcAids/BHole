using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    float damage;
    TrailRenderer trail;
    Rigidbody rigid;

    public void Init(float damage, float force)
    {
        this.damage = damage;
        rigid = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        rigid.velocity=transform.forward *force;
        Destroy(gameObject,2);
    }

    private void OnTriggerEnter(Collider other)
    {
        ITakeDamage target = other.gameObject.GetComponent<ITakeDamage>();
        if (target!=null)
        {
            target.OnDamageTaken(damage);
        }
        rigid.velocity = Vector3.zero;
        rigid.useGravity = true;
        trail.enabled = false;
    }
}
