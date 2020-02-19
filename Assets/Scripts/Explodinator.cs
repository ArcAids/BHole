using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodinator : MonoBehaviour
{
    [SerializeField]
    float explosionForce=20;
    [SerializeField]
    float radius=2;
    [SerializeField]
    float upwardMultiplier=1.5f;
    List<Rigidbody> parts;
    void Start()
    {
        foreach (var item in transform.GetComponentsInChildren<MeshRenderer>(true))
        {
            MeshCollider coll = item.gameObject.GetComponent<MeshCollider>();
            if (coll == null)
                item.gameObject.AddComponent<MeshCollider>().convex = true;
            else
                coll.convex = true;
            item.gameObject.AddComponent<Rigidbody>();
        }
        parts = new List<Rigidbody>();
        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = true;
            parts.Add(item);
        }
    }

    [ContextMenu("Explode")]
    public void Explode()
    {
        foreach (var item in parts)
        {
            if(item.gameObject.activeSelf)
            {
                item.transform.parent = null;
                item.isKinematic = false;
                item.AddExplosionForce(explosionForce,transform.position,radius,upwardMultiplier,ForceMode.Impulse);
            }
        } 
    }
}
