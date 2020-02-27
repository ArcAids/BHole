using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveCore : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("coreCollision");
        Rigidbody target = other.gameObject.GetComponent<Rigidbody>();
        if (target == null)
        {
            transform.parent.gameObject.SetActive(false);
            Destroy(transform.parent.gameObject);
        }
    }
}
