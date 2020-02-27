using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    float damage;
    TrailRenderer trail;
    protected Rigidbody rigid;

    public virtual void Init(float damage, float force)
    {
        this.damage = damage;
        rigid = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        rigid.velocity=transform.forward *force;
        Destroy(gameObject,2);
    }

    protected virtual void OnTriggerEnter(Collider other)
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
