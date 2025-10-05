using System;

public interface IGameMode
{
    void StartMode();
    event Action Victory;
    event Action Defeat;
}
