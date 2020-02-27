using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReSpawner : MonoBehaviour
{
    Vector3 position;
    Vector3 scale;
    Quaternion rotation;
    Rigidbody rigid;
    Collider col;
    [SerializeField]
    UnityEvent Enabled;
    private void Awake()
    {
        
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        if(rigid)
            rigid.velocity = Vector3.zero;
        if (col)
            col.isTrigger = false;
        transform.position = position;
        transform.localScale= scale;
        transform.rotation = rotation;
        Invoke("Enabler", 5);
    }

    void Enabler()
    {
        gameObject.SetActive(true);
        Enabled.Invoke();
    }
}
