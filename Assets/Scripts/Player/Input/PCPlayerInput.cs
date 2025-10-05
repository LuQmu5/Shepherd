using UnityEngine;

public class PCPlayerInput : IPlayerInput
{
    public float Horizontal => Input.GetAxisRaw("Horizontal");
    public float Vertical => Input.GetAxisRaw("Vertical");
    public bool Jump => Input.GetButtonDown("Jump");
    public bool FireHeld => Input.GetMouseButtonDown(0);
    public bool FireReleased => Input.GetMouseButtonUp(0);
    public float MouseX => Input.GetAxis("Mouse X");
    public float MouseY => Input.GetAxis("Mouse Y");
}
