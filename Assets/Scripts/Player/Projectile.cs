using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _launchForce = 20f;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private float _explosionRange = 10;
    [SerializeField] private ParticleSystem _explosionVFX;

    private float _timeAlive;
    private float _shotPower;
    private float _creationTime;

    private void Awake()
    {
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody>();
    }

    public void Launch(float shotPower)
    {
        _creationTime = Time.time;
        _shotPower = shotPower;
        _launchForce = shotPower;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.useGravity = true;

        _rigidbody.AddForce(transform.forward * _launchForce, ForceMode.Impulse);

        _timeAlive = 0f;
    }

    private void Update()
    {
        _timeAlive += Time.deltaTime;

        if (_rigidbody.linearVelocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.linearVelocity.normalized);
        }

        if (_timeAlive > _lifeTime)
        {
            Explode();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.gameObject.name);
        Explode();
        gameObject.SetActive(false);
    }

    private void Explode()
    {
        ParticleSystem explosion = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        Destroy(explosion, explosion.main.duration);

        var hits = Physics.OverlapSphere(transform.position, _explosionRange);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IHealth health))
            {
                health.ApplyDamage(_shotPower);
            }

            if (hit.TryGetComponent(out Rigidbody rigidbody))
            {
                if (Mathf.Abs(rigidbody.linearVelocity.y) > 0)
                    return;

                
                float multiplier = Vector3.Distance(transform.position, rigidbody.transform.position);
                multiplier = Mathf.Clamp(multiplier, 0.25f, 2);
                Vector3 force = (-rigidbody.transform.forward * (_shotPower / (multiplier * 4)) + (Vector3.up * (_shotPower / multiplier))) * 0.3f;
                rigidbody.AddForce(force, ForceMode.Impulse);
            }
        }

        
    }
}
