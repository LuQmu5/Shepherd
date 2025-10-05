using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ControlPopulationMode : IGameMode
{
    private readonly MonoBehaviour _coroutineRunner;
    private readonly PlayerController _player;
    private readonly EnemyFactory _factory;

    private readonly List<EnemyController> _spawnedEnemies = new();

    private float _duration;
    private int _maxEnemiesAlive; 
    private float _spawnRadius = 25f;
    private float _spawnDelay = 2f; 
    private float _minSpawnDelay = 0.3f; 
    private float _spawnAcceleration = 0.05f; 
    private int _baseEnemiesPerSpawn = 1;
    private int _waveIncrement = 1; 

    private float _timeElapsed;
    private int _currentWave = 0;

    public event Action Victory;
    public event Action Defeat;

    public ControlPopulationMode(MonoBehaviour coroutineRunner, EnemyFactory factory, PlayerController player, float duration = 10, int maxEnemiesAlive = 5)
    {
        _coroutineRunner = coroutineRunner;
        _factory = factory;
        _duration = duration;
        _maxEnemiesAlive = maxEnemiesAlive;

        _player = player;
        _player.Dead += () => Defeat?.Invoke();
    }

    public void StartMode()
    {
        Debug.Log($"Timed Survival Mode Started: survive {_duration} seconds, max {_maxEnemiesAlive} enemies alive.");
        _timeElapsed = 0f;
        _currentWave = 0;

        _coroutineRunner.StartCoroutine(TimeCounting());
        _coroutineRunner.StartCoroutine(SpawnWaveRoutine());
    }


    private IEnumerator TimeCounting()
    {
        while (_timeElapsed < _duration)
        {
            _timeElapsed += Time.deltaTime;

            int aliveEnemies = _spawnedEnemies.Count(e => !e.IsDead);
            Debug.Log($"Time left: {_duration - _timeElapsed}, Enemies: {aliveEnemies}/{_maxEnemiesAlive}");

            if (aliveEnemies > _maxEnemiesAlive)
            {
                Defeat?.Invoke();
                yield break;
            }

            yield return null;
        }

        Victory?.Invoke();
    }

    private IEnumerator SpawnWaveRoutine()
    {
        float spawnDelay = 2f; 
        float minSpawnDelay = 0.3f; 
        int baseEnemiesPerSpawn = 2; 
        int maxEnemiesPerSpawn = 5; 
        float difficultyProgress = 0f; 

        while (_timeElapsed < _duration)
        {
            difficultyProgress = Mathf.Clamp01(_timeElapsed / _duration);
            int enemiesToSpawn = Mathf.RoundToInt(Mathf.Lerp(baseEnemiesPerSpawn, maxEnemiesPerSpawn, difficultyProgress));
            float currentSpawnDelay = Mathf.Lerp(spawnDelay, minSpawnDelay, difficultyProgress);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy(difficultyProgress);
                yield return new WaitForSeconds(currentSpawnDelay);
            }

            yield return null;
        }
    }

    private void SpawnEnemy(float difficultyProgress)
    {
        EnemyController enemy = _factory.Get();
        enemy.Init(_player, maxHealth: 10);
        enemy.SetBehavior(new FleeBehavior());

        float moduleChance = Mathf.Lerp(0.2f, 0.8f, difficultyProgress); 
        int maxExtraModules = 2;

        for (int m = 0; m < maxExtraModules; m++)
        {
            if (Random.value < moduleChance)
            {
                if (!enemy.TryAddRandomModule())
                    break;
            }
        }

        enemy.transform.position = GetRandomPointFromCenterScene(_spawnRadius);
        enemy.gameObject.SetActive(true);

        _spawnedEnemies.Add(enemy);
    }


    private Vector3 GetRandomPointFromCenterScene(float radius)
    {
        var angle = Random.Range(0f, Mathf.PI * 2f);
        var r = Random.Range(0f, radius);
        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * r;
    }
}
