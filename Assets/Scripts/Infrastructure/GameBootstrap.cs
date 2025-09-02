using Unity.Cinemachine;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Projectile _projectilePrefab;

    private void Awake()
    {
        ProjectileFactory factory = new ProjectileFactory(_projectilePrefab);

        PlayerController playerInstance = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        playerInstance.Init(factory);
    }
}
