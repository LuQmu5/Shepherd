using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private ParticleSystem _deathVFX;

    private IEnemyBehavior _behavior;
    private readonly List<IEnemyModule> _modules = new();
    private float _currentHealth;

    public PlayerController Player { get; private set; }
    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;

    public bool IsDead => _currentHealth == 0;

    public void Init(PlayerController player, float maxHealth = 10)
    {
        _currentHealth = maxHealth;
        Player = player;
    }

    private void Update()
    {
        _animator.SetBool("IsRunning", _agent.hasPath);

        _behavior?.Tick(Time.deltaTime);

        foreach (var module in _modules)
            module.Tick(Time.deltaTime);
    }

    public void SetBehavior(IEnemyBehavior behavior)
    {
        _behavior = behavior;
        _behavior.Initialize(this);
    }

    public void AddModule(IEnemyModule module)
    {
        _modules.Add(module);
        module.Apply(this);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead)
            return;

        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Instantiate(_deathVFX, transform.position + Vector3.up, Quaternion.identity);

        foreach (var module in _modules)
            module.Remove();

        _modules.Clear();

        gameObject.SetActive(false);
    }


    public bool TryAddRandomModule()
    {
        IEnemyModule[] allModules =
        {
            new SpeedModule(),
            new InvisibleModule(),
        };

        var availableModules = allModules.Where(m => !_modules.Any(existing => existing.GetType() == m.GetType())).ToArray();

        if (availableModules.Length == 0)
            return false; 

        var randomModule = availableModules[Random.Range(0, availableModules.Length)];

        AddModule(randomModule);
        return true;
    }

}
