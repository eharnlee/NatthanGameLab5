using UnityEngine;

public interface IPowerUp
{
    void DestroyPowerup();
    void SpawnPowerup();
    void ApplyPowerup(MonoBehaviour i);

    PowerUpType powerUpType
    {
        get;
    }

    bool hasSpawned
    {
        get;
    }
}


public interface IPowerupApplicable
{
    public void RequestPowerupEffect(IPowerUp i);
}