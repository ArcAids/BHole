using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    LayerMask targetables;
    [SerializeField]
    GameObject followThis;
    [SerializeField]

    Camera cam;
    PlayerMovement playerMovement;
    FirstPersonShooter fpsController;

    IPlayerMovementInput input;
    Plane movementPlane;

    private void Start()
    {
        input = new PlayerInput();
        movementPlane = new Plane(Vector3.up, transform.position);
        playerMovement = GetComponent<PlayerMovement>();
        fpsController = GetComponent<FirstPersonShooter>();
    }

    private void Update()
    {
        input.GetPlayerInput();

        playerMovement.Move(input.Vertical, input.Horizontal, input.Dash);
        if(followThis)
            playerMovement.LookToward(followThis.transform.position -transform.position);
        else
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (movementPlane.Raycast(ray, out float point))
            {
                Vector3 hit = ray.GetPoint(point);
                playerMovement.LookToward(hit - transform.position);
            }
        }
        fpsController?.UpdateControls();
    }

    private void OnTriggerEnter(Collider other)
    {
        IPickable pickable= other.gameObject.GetComponent<IPickable>();
        pickable?.Pick();
        WeaponBehaviour weapon = other.GetComponent<WeaponBehaviour>();
        if (weapon != null)
            fpsController.PickUpWeapon(weapon);
    }
}
