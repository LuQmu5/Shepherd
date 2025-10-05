using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ParticleSystem _explosionVFX;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private float _explosionRange = 10;

    private float _timeAlive;
    private float _shotPower;

    public void Launch(float shotPower)
    {
        _shotPower = shotPower;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.useGravity = true;

        _rigidbody.AddForce(transform.forward * _shotPower, ForceMode.Impulse);
        _timeAlive = 0f;
    }

    private void Update()
    {
        _timeAlive += Time.deltaTime;

        if (_rigidbody.linearVelocity.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(_rigidbody.linearVelocity.normalized);

        if (_timeAlive > _lifeTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.gameObject.name);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Explode();
    }

    private void Explode()
    {
        ParticleSystem explosion = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        Destroy(explosion, explosion.main.duration);

        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRange);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out PlayerController player) && hit.TryGetComponent(out Rigidbody rigidbody))
            {
                if (player.OnGround)
                    rigidbody.AddForce(Vector2.up * _shotPower * _shotPower);
            }

            if (hit.TryGetComponent(out EnemyController enemy))
            {
                enemy.TakeDamage(_shotPower);
            }
        }   
    }
}
