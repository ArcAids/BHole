using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHole : BulletBehaviour
{
    [SerializeField] protected float stabilityTime=5;
    protected List<Rigidbody> objectsInRange = new List<Rigidbody>();
    public override void Init(float damage, float force)
    {
        StopAllCoroutines();
        objectsInRange.Clear();
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(Evaporation());
      

    }

    protected virtual IEnumerator Evaporation()
    {
        float timer = stabilityTime;
        while (timer >= 0)
        {
            yield return null;
            Vector3 newScale=Vector3.one * Mathf.Pow(timer / stabilityTime,0.3f);
            newScale*=0.5f;
            newScale.z = 1;
            transform.localScale = newScale ;
            timer -= Time.deltaTime;
        }
        foreach (var rigid in objectsInRange)
        {
            rigid.velocity = Vector3.zero;
            rigid.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }



    protected override void OnTriggerEnter(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        Collider collider = other.GetComponent<Collider>();
        if (rigid != null && !rigid.isKinematic && !collider.isTrigger)
        {
            if(other.CompareTag("Player"))
            {
                GameManager.Instance.GameOverFromWormHole();
                other.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled=false;
                other.gameObject.GetComponent<Collider>().enabled=false;
            }
            else
                collider.isTrigger = true;

            objectsInRange.Add(rigid);
            rigid.useGravity = false;
            
            rigid.velocity = (transform.position - rigid.position).normalized * 10;
        }
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if (rigid != null && !other.CompareTag("Player"))
        {
            if (objectsInRange.Contains(rigid)){ rigid.gameObject.SetActive(false); }
                objectsInRange.Remove(rigid);
        }
    }
}
