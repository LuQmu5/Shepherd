using UnityEngine;

public class NinjaSheep : MonoBehaviour, IHealth
{
    [SerializeField] private ParticleSystem _deathVFX;
    [SerializeField] private Renderer[] rends;

    [Header("Detection")]
    public float detectionRadius = 5f;   // Радиус, в котором овца реагирует на игрока
    public float chargeTime = 3f;        // Время "зарядки" до взрыва

    [Header("Explosion")]
    public float explosionForce = 15f;   // Сила отталкивания
    public float explosionRadius = 6f;   // Радиус взрыва
    public GameObject explosionEffect;   // Визуальный эффект взрыва (по желанию)

    [Header("Visuals")]
    public Color calmColor = Color.white;
    public Color angryColor = Color.red;
    public float scaleIncrease = 1.5f;   // Во сколько раз увеличивается перед взрывом

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
            // Начинаем зарядку
            charging = true;
            timer += Time.deltaTime;

            // Мигаем цветом
            float t = Mathf.PingPong(Time.time * 5f, 1f);

            foreach (var rend in rends)
                rend.material.color = Color.Lerp(calmColor, angryColor, t);

            // Увеличиваемся в размере постепенно
            float scaleFactor = Mathf.Lerp(1f, scaleIncrease, timer / chargeTime);
            transform.localScale = originalScale * scaleFactor;

            // Взрыв
            if (timer >= chargeTime)
            {
                Explode();
            }
        }
        else
        {
            // Спокойное состояние
            charging = false;
            timer = 0f;
            transform.localScale = originalScale;

            foreach (var rend in rends)
                rend.material.color = calmColor;
        }
    }

    void Explode()
    {
        // Визуальный эффект
        if (explosionEffect)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Находим все объекты вокруг
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        // Уничтожаем овцу после взрыва
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Рисуем радиус в редакторе
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
