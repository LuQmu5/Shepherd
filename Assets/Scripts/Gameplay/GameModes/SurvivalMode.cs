using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SurvivalMode : IGameMode
{
    private readonly MonoBehaviour _coroutineRunner;
    private readonly EnemyFactory _factory;
    private readonly PlayerController _player;

    private int _baseEnemyCount = 1;
    private float _spawnRadius = 25f;
    private float _spawnDelayBetweenEnemies = 0.3f;
    private float _prepTimeBeforeWave = 5f;

    private List<EnemyController> _spawnedEnemies = new();
    private int _currentWave;

    public event Action Victory;
    public event Action Defeat;

    public SurvivalMode(MonoBehaviour coroutineRunner, EnemyFactory factory, PlayerController player)
    {
        _coroutineRunner = coroutineRunner;
        _factory = factory;
        _player = player;

        _player.Dead += () => Defeat?.Invoke();
    }

    public void StartMode()
    {
        Debug.Log("Survival Mode Started");
        _currentWave = 0;

        _coroutineRunner.StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        int wavesCount = 2;

        while (_currentWave != wavesCount)
        {
            _currentWave++;
            Debug.Log($"Preparing for wave {_currentWave}...");

            yield return new WaitForSeconds(_prepTimeBeforeWave);

            Debug.Log($"Spawning wave {_currentWave}");

            int enemiesToSpawn = _baseEnemyCount + (_currentWave * 2);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy(_currentWave);
                yield return new WaitForSeconds(_spawnDelayBetweenEnemies);
            }

            bool isAllEnemiesDead = false;

            while (isAllEnemiesDead == false)
            {
                int deadEnemies = _spawnedEnemies.Where(i => i.IsDead).Count();
                Debug.Log("Dead enemies count: " + deadEnemies + " All enemies: " + _spawnedEnemies.Count);
                isAllEnemiesDead = deadEnemies == _spawnedEnemies.Count;

                yield return null;
            }

            _spawnedEnemies.Clear();
        }

        Victory?.Invoke();
    }

    private void SpawnEnemy(int waveNumber)
    {
        float baseHealth = 10;
        float spawnRadius = 25;

        EnemyController enemy = _factory.Get();
        enemy.Init(_player, baseHealth);
        enemy.SetBehavior(new AggressiveBehavior());

        int extraModulesCount = Random.Range(0, waveNumber);

        while (extraModulesCount > 0) 
        {
            if (enemy.TryAddRandomModule() == false)
            {
                break;
            }

            extraModulesCount--;
        }

        enemy.gameObject.SetActive(true);
        enemy.transform.position = GetRandomPointFromCenterScene(spawnRadius);

        _spawnedEnemies.Add(enemy);
    }

    private Vector3 GetRandomPointFromCenterScene(float radius)
    {
        var angle = Random.Range(0f, Mathf.PI * 2f);
        var r = Random.Range(0f, radius);
        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * r;
    }
}
