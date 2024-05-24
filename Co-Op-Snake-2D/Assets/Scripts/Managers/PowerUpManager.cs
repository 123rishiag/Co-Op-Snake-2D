using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private static PowerUpManager instance;
    public static PowerUpManager Instance { get { return instance; } }

    private Camera mainCamera;
    private int viewportWidth;
    private int viewportHeight;

    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 6f;
    public float powerUpLifetime = 10f;
    public PowerUp[] powerUps;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        viewportWidth = (int)(mainCamera.aspect * mainCamera.orthographicSize * 2f);
        viewportHeight = (int)(mainCamera.orthographicSize * 2f);
        Invoke("SpawnPowerUp", UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    private GameObject GetPowerUpPrefab(PowerUpType powerUpType)
    {
        PowerUp powerUp = Array.Find(powerUps, item => item.powerUpType == powerUpType);
        return powerUp?.powerUpPrefab;
    }

    public PowerUpType GetPowerUpType(string powerUpPrefabName)
    {
        powerUpPrefabName = powerUpPrefabName.Replace("(Clone)", "");
        PowerUp powerUp = Array.Find(powerUps, item => item.powerUpPrefab.name == powerUpPrefabName);
        return powerUp?.powerUpType ?? PowerUpType.NoPowerUp;
    }

    private void SpawnPowerUp()
    {
        PowerUpType powerUpType = GetRandomPowerUpType();
        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject powerUpPrefab = GetPowerUpPrefab(powerUpType);

        if (powerUpPrefab != null)
        {
            GameObject powerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity, transform);
            Destroy(powerUp, powerUpLifetime);
        }

        Invoke("SpawnPowerUp", UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    private PowerUpType GetRandomPowerUpType()
    {
        float randomValue = UnityEngine.Random.value;
        if (randomValue <= 0.33f)
        {
            return PowerUpType.ScoreBoostPowerUp;
        }
        if (randomValue <= 0.66f)
        {
            return PowerUpType.ShieldPowerUp;
        }
        return PowerUpType.SpeedUpPowerUp;
    }

    private Vector2 GetRandomSpawnPosition()
    {
        return new Vector2(UnityEngine.Random.Range(-viewportWidth / 2f, viewportWidth / 2f),
                           UnityEngine.Random.Range(-viewportHeight / 2f, viewportHeight / 2f));
    }
}

[Serializable]
public class PowerUp
{
    public PowerUpType powerUpType;
    public GameObject powerUpPrefab;
}

public enum PowerUpType
{
    ScoreBoostPowerUp,
    ShieldPowerUp,
    SpeedUpPowerUp,
    NoPowerUp
}
