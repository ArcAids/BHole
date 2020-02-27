using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBullet : BulletBehaviour
{
    float forcePassed=0.4f;
    protected override void OnTriggerEnter(Collider other)
    {
        Rigidbody target = other.gameObject.GetComponent<Rigidbody>();
        if (target != null)
        {
            target.AddForce(rigid.velocity * forcePassed, ForceMode.Impulse);
        }
    }
}
