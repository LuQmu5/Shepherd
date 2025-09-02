using UnityEngine;
using UnityEngine.AI;

public class AggressiveGoat : MonoBehaviour, IHealth
{
    [Header("Movement")]
    public float acceleration = 2f;       // скорость набора скорости
    public float maxSpeed = 6f;           // максимальная скорость
    public float stopDistance = 2f;       // дистанция до игрока, на которой остановиться

    [Header("Attack")]
    public float attackCooldown = 2f;     // задержка между атаками
    public float attackForce = 10f;       // сила отталкивания игрока

    [SerializeField] private ParticleSystem _deathVFX;

    [SerializeField] private Animator _animator;

    private NavMeshAgent agent;
    private Transform player;
    private float currentSpeed = 0f;
    private float attackTimer = 0f;

    public void Init(Transform p)
    {
        agent = GetComponent<NavMeshAgent>();
        player = p;

        // выключаем автоуправление скоростью, будем сами
        agent.speed = 0;
        agent.acceleration = 1000f; // ставим большое, чтобы не мешало нашему управлению
    }

    void Update()
    {
        if (!player) 
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            // разгоняемся
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

            agent.isStopped = false;
            agent.speed = currentSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            // остановка рядом с игроком
            agent.isStopped = true;
            currentSpeed = 0f;

            // атака
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    void Attack()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            rb.AddForce(Vector3.up * attackForce / 2 + dir * attackForce, ForceMode.Impulse);
            _animator.SetTrigger("Attack");
        }
    }

    public void ApplyDamage(float damage)
    {
        Instantiate(_deathVFX, transform.position + Vector3.up, Quaternion.identity);
        Destroy(gameObject);
    }
}
