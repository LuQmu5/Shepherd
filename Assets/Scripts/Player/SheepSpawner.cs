using UnityEngine;
using UnityEngine.AI;

public class SheepSpawner : MonoBehaviour
{
    [SerializeField] private SheepController _sheepPrefab;
    [SerializeField] private FlyingSheep _flyingSheepPrefab;
    [SerializeField] private AggressiveGoat _aggressiveGoatPrefab;
    [SerializeField] private NinjaSheep _ninjaSheepPrefab;

    [Header("Настройки спавна")]
    [SerializeField] private float _radius = 5f;     
    [SerializeField] private float _spawnInterval = 1f; 

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= _spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    private void SpawnObject()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _radius;
        Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);


        if (Random.Range(0, 100) < 60)
        {
            int randomNumber = Random.Range(1, 4);

            if (randomNumber == 1)
            {
                SheepController sheep = Instantiate(_sheepPrefab, spawnPos, Quaternion.identity);

                var agent = sheep.GetComponent<NavMeshAgent>();

                if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                }
            }
            else if (randomNumber == 2)
            {
                NinjaSheep ninja = Instantiate(_ninjaSheepPrefab, spawnPos, Quaternion.identity);
                ninja.Init(FindAnyObjectByType<PlayerController>().transform);
            }
            else
            {
                AggressiveGoat goat = Instantiate(_aggressiveGoatPrefab, spawnPos, Quaternion.identity);
                goat.Init(FindAnyObjectByType<PlayerController>().transform);

                var agent = goat.GetComponent<NavMeshAgent>();

                if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                }
            }

        }
        else
        {
            FlyingSheep sheep = Instantiate(_flyingSheepPrefab, spawnPos + Vector3.up * Random.Range(10, 20), Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
