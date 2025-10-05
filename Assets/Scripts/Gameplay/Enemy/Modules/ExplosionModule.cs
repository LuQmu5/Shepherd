public class ExplosionModule : IEnemyModule
{
    private EnemyController _enemy;

    public void Apply(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Remove()
    {
        
    }

    public void Tick(float deltaTime)
    {
        
    }
}
