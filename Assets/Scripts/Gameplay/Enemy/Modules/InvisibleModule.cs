using UnityEngine;
using System.Collections.Generic;

public class InvisibleModule : IEnemyModule
{
    private readonly Dictionary<Renderer, Color> _baseColorsMap = new();

    private Color _invisibleColor = new Color(0.75f, 1, 0.75f, 0.25f);

    public void Apply(EnemyController enemy)
    {
        foreach (Renderer renderer in enemy.GetComponentsInChildren<Renderer>())
        {
            _baseColorsMap[renderer] = renderer.material.color;
            renderer.material.color = _invisibleColor;
        }
    }

    public void Remove()
    {
        foreach (Renderer renderer in _baseColorsMap.Keys)
        {
            renderer.material.color = _baseColorsMap[renderer];
        }
    }

    public void Tick(float deltaTime)
    {
        
    }
}
