using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    float dashRechargeTime;
    [SerializeField]
    float dashBoost;

    bool canDash=true;

    TrailRenderer dashTrail;
    CharacterController movementController;

    Vector3 movement;
    Vector3 targetRotation;

    private void Awake()
    {
        movementController = GetComponent<CharacterController>();
        dashTrail = GetComponentInChildren<TrailRenderer>();
    }

    public void Move(float vertical, float horizontal,bool dash)
    {
        if (vertical == 0 && horizontal == 0)
            return;

        movement.z = vertical;
        movement.x = horizontal;
        movement = movement.normalized * Time.deltaTime * speed;

        if (dashTrail != null && dashTrail.emitting)
            dashTrail.emitting = false;

        if(dash && canDash)
        {
            movement *= dashBoost;
            if(dashTrail) dashTrail.emitting = true;
            StartCoroutine(DashRechargeTimer());
        }
        movementController.Move(movement);

    }

    public void LookToward(Vector3 direction)
    {
        targetRotation = direction;
        targetRotation.y = 0;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(targetRotation, transform.up), Time.deltaTime* rotationSpeed);
    }

    IEnumerator DashRechargeTimer()
    {
        canDash = false;
        float timer = dashRechargeTime;
        while(timer>0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        canDash = true;
    }
}
