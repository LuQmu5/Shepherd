using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpeedModule : IEnemyModule
{
    private EnemyController _enemy;
    private float _multiplier = 5f;
    private float _baseSpeed;

    public void Apply(EnemyController enemy)
    {
        _enemy = enemy;
        _baseSpeed = _enemy.Agent.speed;
        _enemy.Agent.speed *= _multiplier;
    }

    public void Remove()
    {
        _enemy.Agent.speed = _baseSpeed;
    }

    public void Tick(float deltaTime)
    {
       
    }
}
