using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class ProjectileFactory
{
    private Projectile _projectilePrefab;
    private List<Projectile> _pool;

    public ProjectileFactory(Projectile projectilePrefab)
    {
        _projectilePrefab = projectilePrefab;

        _pool = new List<Projectile>();
    }

    public Projectile Get()
    {
        Projectile item = _pool.Find(i => i.gameObject.activeSelf == false);

        if (item == null)
        {
            item = Object.Instantiate(_projectilePrefab);
            _pool.Add(item);
        }

        return item;
    }
}