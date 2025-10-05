public interface IEnemyBehavior
{
    void Initialize(EnemyController enemy);
    void Tick(float deltaTime);
}
