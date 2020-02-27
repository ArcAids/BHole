using System.Collections.Generic;
using UnityEngine;

public class PressureTile : MonoBehaviour
{
    [SerializeField] GameObject door;
    
    protected List<Rigidbody> objectsInRange = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && !other.attachedRigidbody.isKinematic)
        {
            objectsInRange.Add(other.attachedRigidbody);
            door.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
        {
            objectsInRange.Remove(other.attachedRigidbody);
            if(objectsInRange.Count==0)
            {
                door.SetActive(false);
            }
        }
    }

}
