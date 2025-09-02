using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class SheepController : MonoBehaviour, IHealth
{
    [Header("Wander Settings")]
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float minIdleTime = 2f;
    [SerializeField] private float maxIdleTime = 5f;

    [Header("Speed Settings")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3.5f;

    [SerializeField] private ParticleSystem _deathVFX;
    [SerializeField] private Animator _animator;

    private NavMeshAgent agent;
    private Coroutine behaviorRoutine;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        behaviorRoutine = StartCoroutine(WanderRoutine());
    }

    private void Update()
    {
        _animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            // Случайная скорость
            agent.speed = Random.Range(minSpeed, maxSpeed);

            // Случайная точка в радиусе
            Vector3 destination = GetRandomPointOnNavMesh(transform.position, wanderRadius);

            agent.SetDestination(destination);

            // Ждём, пока овца придет или застрянет
            while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;
            }

            // Остановиться на случайное время
            agent.isStopped = true;
            float idleTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(idleTime);
            agent.isStopped = false;
        }
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++) // попытки найти точку
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection.y = 0;
            Vector3 target = center + randomDirection;

            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return center; // fallback
    }

    public void ApplyDamage(float damage)
    {
        ParticleSystem vfx = Instantiate(_deathVFX, transform.position + Vector3.up, Quaternion.identity);
        // Destroy(vfx, vfx.main.duration);

        Destroy(gameObject);
    }
}
