using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMotor _motor;
    [SerializeField] private PlayerLook _look;
    [SerializeField] private PlayerJump _jump;
    [SerializeField] private RocketLauncher _rocketLauncher;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _legsPoint;
    [SerializeField] private LayerMask _groundMask;

    private IPlayerInput _input;
    private float _currentHealth = 4;

    public bool OnGround => Physics.OverlapSphere(_legsPoint.position, 0.5f, _groundMask).Length > 0;
    public event Action Dead;

    public void Init(IPlayerInput input)
    {
        _input = input;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _motor.SetMoveInput(transform.right * _input.Horizontal + transform.forward * _input.Vertical);
        _look.Look(_input.MouseX, _input.MouseY);
        _jump.TryJump(_input.Jump);

        if (_input.FireHeld)
            _rocketLauncher.StartCharge();

        if (_input.FireReleased)
        {
            _rocketLauncher.ReleaseCharge();

            if (OnGround)
                ApplyRecoil();
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Dead?.Invoke();
        gameObject.SetActive(false);
    }

    private void ApplyRecoil()
    {
        float recoilCoeff = 0.2f;
        float recoilPower = _rocketLauncher.CurrentPower * recoilCoeff;
        Vector3 recoilDirection = transform.forward * -1 + transform.up;
        _rigidbody.AddForce(recoilDirection * recoilPower, ForceMode.Impulse);
    }
}
