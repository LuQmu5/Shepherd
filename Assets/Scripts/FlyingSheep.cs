using UnityEngine;

public class FlyingSheep : MonoBehaviour, IHealth
{
    [SerializeField] private ParticleSystem _deathVFX;

    public float speed = 5f;              // �������� �����
    public float rotationSpeed = 2f;      // �������� ��������
    public float changeDirectionTime = 3f;// ��� � ������� ������ ������ �����������
    public float flightHeight = 10f;      // ������� ������ �����
    public float bobbingAmplitude = 1f;   // ��������� "�����������"
    public float bobbingSpeed = 2f;       // �������� "�����������"

    private Vector3 targetDirection;      // ���� �����
    private float timer;

    void Start()
    {
        PickNewDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // ���� ������ ����� � �������� ����� �����������
        if (timer >= changeDirectionTime)
        {
            PickNewDirection();
            timer = 0f;
        }

        // ������� � ������� ����� ����
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // �������� �����
        transform.position += transform.forward * speed * Time.deltaTime;

        // ������������ ���� �� ������ ������ + "�����������"
        float bobbing = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmplitude;
        Vector3 pos = transform.position;
        pos.y = flightHeight + bobbing;
        transform.position = pos;
    }

    void PickNewDirection()
    {
        // �������� ��������� ����������� �� �����������
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
