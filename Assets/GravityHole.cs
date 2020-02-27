using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHole : WormHole
{
    [SerializeField] float gravityStrength = 5;

    protected override IEnumerator Evaporation()
    {
        Coroutine pull=StartCoroutine(Pull());
        float timer = stabilityTime;
        while (timer >= 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            transform.localScale = Vector3.one * (timer / stabilityTime);
        }
        StopCoroutine(pull);
        foreach (var rigid in objectsInRange)
        {
            rigid.velocity = Vector3.zero;
        }
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if (rigid != null)
            objectsInRange.Add(rigid);
    }

    protected override void OnTriggerExit(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            objectsInRange.Remove(rigid);
        }
    }

    IEnumerator Pull()
    {
        while (true)
        {
            foreach (var rigid in objectsInRange)
            {
                float gravity = gravityStrength * rigid.mass / Vector3.SqrMagnitude(transform.position - rigid.position);

                gravity = Mathf.Clamp(gravity, 0, 10);
                rigid.velocity = (transform.position - rigid.position).normalized * gravity;
            }
            yield return new WaitForFixedUpdate();
        }
    }

}    
