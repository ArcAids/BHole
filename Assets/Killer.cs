using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            if (other.CompareTag("Player"))
                GameManager.Instance.GameOver();
            other.gameObject.SetActive(false);
        }
    }
}
