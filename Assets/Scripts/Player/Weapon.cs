using UnityEngine;

public class Weapon
{
    private readonly ProjectileFactory _factory;
    private readonly Transform _shootPoint;

    private float _shotPower;

    public Weapon(ProjectileFactory factory, Transform shootPoint, float shootPower)
    {
        _factory = factory;
        _shootPoint = shootPoint;
        _shotPower = shootPower;
    }

    public void Shoot(float multiplier)
    {
        Projectile projectile = _factory.Get();

        projectile.transform.parent = _shootPoint;
        projectile.transform.localPosition = Vector3.zero;
        projectile.transform.localEulerAngles = Vector3.zero;
        projectile.transform.parent = null;
        projectile.gameObject.SetActive(true);

        projectile.Launch(_shotPower * multiplier);
    }
}
