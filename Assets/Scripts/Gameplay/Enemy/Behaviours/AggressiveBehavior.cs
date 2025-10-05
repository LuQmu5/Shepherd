using UnityEngine;
using UnityEngine.AI;

public class AggressiveBehavior : IEnemyBehavior
{
    private EnemyController _controller;
    private Transform _player;

    private float _attackDistance = 5;
    private float _attackCooldown = 2;
    private float _damage = 2;

    private float _currentAttackCooldown;

    public void Initialize(EnemyController controller)
    {
        _controller = controller;
        _player = controller.Player.transform;
        _currentAttackCooldown = _attackCooldown;
    }

    public void Tick(float deltaTime)
    {
        if (_player == null)
            return;

        if (_currentAttackCooldown > 0)
            _currentAttackCooldown -= deltaTime;

        if (NavMesh.SamplePosition(_player.position, out var hit, 2f, 1))
            _controller.Agent.SetDestination(hit.position);

        if (_currentAttackCooldown <= 0 && IsPlayerInAttackRange())
        {
            _currentAttackCooldown = _attackCooldown;
            _controller.Animator.SetTrigger("Attack");
            _controller.Player.TakeDamage(_damage);
        }
    }

    private bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(_controller.transform.position, _player.position) <= _attackDistance;
    }
}

