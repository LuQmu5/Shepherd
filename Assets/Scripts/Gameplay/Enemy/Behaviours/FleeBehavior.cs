using UnityEngine;
using UnityEngine.AI;

public class FleeBehavior : IEnemyBehavior
{
    private EnemyController _enemy;
    private Transform _player;
    private float _fleeDistance = 10f;
    private float _searchRadius = 15f;
    private int _maxAttempts = 10;

    public void Initialize(EnemyController enemy)
    {
        _enemy = enemy;
        _player = enemy.Player.transform;
    }

    public void Tick(float deltaTime)
    {
        if (_player == null)
            return;

        float distance = Vector3.Distance(_player.position, _enemy.transform.position);

        if (distance >= _fleeDistance)
            return;

        _enemy.Agent.SetDestination(GetFleePoint());
    }

    private Vector3 GetFleePoint()
    {
        Vector3 fleeDir = (_enemy.transform.position - _player.position).normalized;
        Vector3 fleePoint = _enemy.transform.position;

        for (int i = 0; i < _maxAttempts; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
            randomOffset.y = 0;

            Vector3 candidate = _enemy.transform.position + (fleeDir + randomOffset).normalized * _fleeDistance;

            if (NavMesh.SamplePosition(candidate, out var hit, _searchRadius, NavMesh.AllAreas))
            {
                fleePoint = hit.position;
                break;
            }
        }

        return fleePoint;
    }
}
