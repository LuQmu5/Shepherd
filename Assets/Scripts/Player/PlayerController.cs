using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera _lookCamera;
    [SerializeField] private WeaponView _weaponView;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 15f;    // ���������
    [SerializeField] private float deceleration = 10f;    // ����������
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform legs;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float verticalLookLimit = 85f;

    [Header("Combat Settings")]
    [SerializeField] private float shootPower = 10f;
    [SerializeField] private float recoilBackForce = 6f;
    [SerializeField] private float recoilUpForce = 3f;

    private Weapon _weapon;
    private float _rotationX = 0f;
    private Vector3 _moveInput;
    private Vector3 _targetVelocity;
    private Coroutine _chargingCoroutine;

    private void Awake()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    public void Init(ProjectileFactory factory)
    {
        Cursor.lockState = CursorLockMode.Locked;
        _weapon = new Weapon(factory, _weaponView.ShootPoint, shootPower);
    }

    private void Update()
    {
        HandleMouseLook();
        HandleInput();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _chargingCoroutine = StartCoroutine(Charging());
        }
    }

    private void Shoot(float multiplier)
    {
        _weaponView.PlayRecoil(multiplier);
        _weapon.Shoot(multiplier);
        ApplyRecoil(multiplier);
    }

    private IEnumerator Charging()
    {
        float maxChargeTime = 2;
        float currentTime = 0.6f;

        while (currentTime < maxChargeTime)
        {
            currentTime += Time.deltaTime * 2;

            if (Input.GetKeyUp(KeyCode.Mouse0))
                break;

            yield return null;
        }

        Shoot(currentTime);
        _chargingCoroutine = null;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        _moveInput = (transform.right * x + transform.forward * z).normalized;

        if (Input.GetButtonDown("Jump") && OnGround())
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleMovement()
    {
        Vector3 desiredVelocity = _moveInput * moveSpeed;
        Vector3 velocity = _rigidbody.linearVelocity;

        Vector3 velocityXZ = new Vector3(velocity.x, 0, velocity.z);

        if (_moveInput.magnitude > 0.1f)
        {
            velocityXZ = Vector3.MoveTowards(velocityXZ, desiredVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            velocityXZ = Vector3.MoveTowards(velocityXZ, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        _rigidbody.linearVelocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -verticalLookLimit, verticalLookLimit);

        _lookCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void ApplyRecoil(float multiplier)
    {
        Vector3 recoilDir = -transform.forward * recoilBackForce + Vector3.up * recoilUpForce;
        _rigidbody.AddForce(recoilDir * multiplier, ForceMode.Impulse);
    }

    private bool OnGround()
    {
        return Physics.CheckSphere(legs.position, 0.2f, groundMask);
    }
}
