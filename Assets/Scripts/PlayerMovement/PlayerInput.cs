using UnityEngine;

public class PlayerInput : IPlayerMovementInput
{
    public float Vertical { get; private set; }
    public float Horizontal { get; private set; }
    public bool Dash { get; private set; }
    public bool Attack { get; private set; }

    public void GetPlayerInput()
    {
        Vertical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
        Attack = Input.GetButtonDown("Fire1");
        Dash = Input.GetButtonDown("Fire2");
    }
}

interface IPlayerMovementInput
{
    float Vertical { get; }
    float Horizontal { get; }
    bool Dash { get; }
    bool Attack { get; }

    void GetPlayerInput();
}
