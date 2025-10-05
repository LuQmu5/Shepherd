
public interface IEnemyModule
{
    void Apply(EnemyController enemy);
    void Tick(float deltaTime);
    void Remove();
}

