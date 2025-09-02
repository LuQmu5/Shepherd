using UnityEngine;

public class NinjaSheep : MonoBehaviour, IHealth
{
    [SerializeField] private ParticleSystem _deathVFX;
    [SerializeField] private Renderer[] rends;

    [Header("Detection")]
    public float detectionRadius = 5f;   // ������, � ������� ���� ��������� �� ������
    public float chargeTime = 3f;        // ����� "�������" �� ������

    [Header("Explosion")]
    public float explosionForce = 15f;   // ���� ������������
    public float explosionRadius = 6f;   // ������ ������
    public GameObject explosionEffect;   // ���������� ������ ������ (�� �������)

    [Header("Visuals")]
    public Color calmColor = Color.white;
    public Color angryColor = Color.red;
    public float scaleIncrease = 1.5f;   // �� ������� ��� ������������� ����� �������

    private Transform player;
    private Vector3 originalScale;
    private float timer = 0f;
    private bool charging = false;

    public void Init(Transform p)
    {
        player = p;
        originalScale = transform.localScale;

        foreach (var rend in rends)
            rend.material.color = calmColor;
    }

    void Update()
    {
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            // �������� �������
            charging = true;
            timer += Time.deltaTime;

            // ������ ������
            float t = Mathf.PingPong(Time.time * 5f, 1f);

            foreach (var rend in rends)
                rend.material.color = Color.Lerp(calmColor, angryColor, t);

            // ������������� � ������� ����������
            float scaleFactor = Mathf.Lerp(1f, scaleIncrease, timer / chargeTime);
            transform.localScale = originalScale * scaleFactor;

            // �����
            if (timer >= chargeTime)
            {
                Explode();
            }
        }
        else
        {
            // ��������� ���������
            charging = false;
            timer = 0f;
            transform.localScale = originalScale;

            foreach (var rend in rends)
                rend.material.color = calmColor;
        }
    }

    void Explode()
    {
        // ���������� ������
        if (explosionEffect)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // ������� ��� ������� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        // ���������� ���� ����� ������
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // ������ ������ � ���������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void ApplyDamage(float damage)
    {
        Instantiate(_deathVFX, transform.position + Vector3.up, Quaternion.identity);
        Destroy(gameObject);
    }
}
