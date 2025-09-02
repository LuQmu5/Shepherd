using UnityEngine;

public class FlyingSheep : MonoBehaviour, IHealth
{
    [SerializeField] private ParticleSystem _deathVFX;

    public float speed = 5f;              // скорость полёта
    public float rotationSpeed = 2f;      // скорость поворота
    public float changeDirectionTime = 3f;// раз в сколько секунд менять направление
    public float flightHeight = 10f;      // базовая высота полёта
    public float bobbingAmplitude = 1f;   // амплитуда "покачивания"
    public float bobbingSpeed = 2f;       // скорость "покачивания"

    private Vector3 targetDirection;      // куда летим
    private float timer;

    void Start()
    {
        PickNewDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // если прошло время — выбираем новое направление
        if (timer >= changeDirectionTime)
        {
            PickNewDirection();
            timer = 0f;
        }

        // поворот в сторону новой цели
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // движение вперёд
        transform.position += transform.forward * speed * Time.deltaTime;

        // поддерживаем овцу на нужной высоте + "покачивание"
        float bobbing = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmplitude;
        Vector3 pos = transform.position;
        pos.y = flightHeight + bobbing;
        transform.position = pos;
    }

    void PickNewDirection()
    {
        // выбираем случайное направление по горизонтали
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        if (randomDir == Vector3.zero)
            randomDir = Vector3.forward;
        targetDirection = randomDir;
    }


    public void ApplyDamage(float damage)
    {
        Instantiate(_deathVFX, transform.position + Vector3.up, Quaternion.identity);
        Destroy(gameObject);    
    }
}
