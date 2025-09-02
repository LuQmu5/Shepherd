using UnityEngine;
using UnityEngine.AI;

public class AggressiveGoat : MonoBehaviour, IHealth
{
    [Header("Movement")]
    public float acceleration = 2f;       // �������� ������ ��������
    public float maxSpeed = 6f;           // ������������ ��������
    public float stopDistance = 2f;       // ��������� �� ������, �� ������� ������������

    [Header("Attack")]
    public float attackCooldown = 2f;     // �������� ����� �������
    public float attackForce = 10f;       // ���� ������������ ������

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

        // ��������� �������������� ���������, ����� ����
        agent.speed = 0;
        agent.acceleration = 1000f; // ������ �������, ����� �� ������ ������ ����������
    }

    void Update()
    {
        if (!player) 
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            // �����������
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

            agent.isStopped = false;
            agent.speed = currentSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            // ��������� ����� � �������
            agent.isStopped = true;
            currentSpeed = 0f;

            // �����
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
