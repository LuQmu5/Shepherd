using UnityEngine;
using UnityEngine.AI;

public class WanderBehavior : IEnemyBehavior
{
    private EnemyController _enemy;
    private float _timer;
    private float _interval = 8f;
    private float _radius = 20f;

    public void Initialize(EnemyController enemy)
    {
        _enemy = enemy;
        PickNewDestination();
    }

    public void Tick(float deltaTime)
    {
        _timer -= deltaTime;

        if (_timer <= 0f)
        {
            PickNewDestination();
        }
    }

    private void PickNewDestination()
    {
        var randomDir = Random.insideUnitSphere * _radius;
        randomDir += _enemy.transform.position;

        if (NavMesh.SamplePosition(randomDir, out var hit, Random.Range(_radius / 2, _radius * 2), 1))
            _enemy.Agent.SetDestination(hit.position);

        _timer = Random.Range(_interval / 2, _interval * 2);
    }
}
