using System;
using Unity.Cinemachine;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private GameModeType _modeType;

    [Header("Player Settings")]
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private PlayerController _playerPrefab;

    private IGameMode _currentMode;
    private EnemyFactory _enemyFactory;
    private PlayerController _playerInstance;

    private void Awake()
    {
        _playerInstance = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        _playerInstance.Init(new PCPlayerInput());

        _enemyFactory = new EnemyFactory();

        _currentMode = CreateMode(_modeType);
        _currentMode.StartMode();
    }

    private void OnEnable()
    {
        _currentMode.Victory += OnVictory;
        _currentMode.Defeat += OnDefeat;
    }

    private void OnDisable()
    {
        _currentMode.Victory -= OnVictory;
        _currentMode.Defeat -= OnDefeat;
    }

    private void OnDefeat()
    {
        print("defeat");
    }

    private void OnVictory()
    {
        print("victory");
    }


    private IGameMode CreateMode(GameModeType type)
    {
        return type switch
        {
            GameModeType.Survival => new SurvivalMode(this, _enemyFactory, _playerInstance),
            GameModeType.ControlPopulation => new ControlPopulationMode(this, _enemyFactory, _playerInstance),
            _ => null
        };
    }
}
