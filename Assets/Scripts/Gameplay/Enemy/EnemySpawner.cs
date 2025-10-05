using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyController[] _enemyPrefabs;
    [SerializeField] private float _radius = 10;

    private Vector3 GetRandomPointInCircle(float radius)
    {
        var angle = Random.Range(0f, Mathf.PI * 2f);
        var r = Random.Range(0f, radius);
        return transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * r;
    }
}
