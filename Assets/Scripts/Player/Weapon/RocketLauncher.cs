using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] private RocketLauncherView _view;
    [SerializeField] private Projectile _projectiletPrefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _minPower = 20f;
    [SerializeField] private float _maxPower = 60f;
    [SerializeField] private float _chargeRate = 20f;

    private float _currentPower;
    private bool _charging;

    public float CurrentPower => _currentPower;

    private void Update()
    {
        if (_charging && _currentPower < _maxPower)
            _currentPower += _chargeRate * Time.deltaTime;
    }

    public void StartCharge()
    {
        _charging = true;
        _currentPower = _minPower;
    }

    public void ReleaseCharge()
    {
        if (!_charging) 
            return;

        Projectile projectile = Instantiate(_projectiletPrefab, _shootPoint.position, _shootPoint.rotation);
        projectile.Launch(_currentPower);

        _view.PlayRecoil(_currentPower);

        _charging = false;
    }
}
