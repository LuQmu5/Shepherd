using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class EnemyFactory
{
    private const string EnemyPrefabsPath = "Prefabs/Enemies";

    private readonly EnemyController[] _prefabs;
    private List<EnemyController> _enemiesPool = new();

    public EnemyFactory()
    {
        _prefabs = Resources.LoadAll<EnemyController>(EnemyPrefabsPath);
    }

    public EnemyController Get()
    {
        EnemyController enemy = _enemiesPool.Find(i => i.gameObject.activeSelf == false);

        if (enemy == null)
        {
            EnemyController randomEnemy = _prefabs[Random.Range(0, _prefabs.Length)];
            enemy = Object.Instantiate(randomEnemy);
            _enemiesPool.Add(enemy);
        }

        return enemy;
    }
}
