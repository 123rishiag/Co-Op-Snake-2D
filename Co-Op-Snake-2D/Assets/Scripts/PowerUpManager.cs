using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private static PowerUpManager instance;
    public static PowerUpManager Instance { get { return instance; } } // Singleton instance accessor.

    private Camera mainCamera;
    private int viewportWidth;
    private int viewportHeight;

    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 6f;
    public float powerUpLifetime = 10f;
    public PowerUp[] powerUps; // Array of powerUps configurations.

    private void Awake()
    {
        // Ensure only one instance of PowerUpManager exists.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction on load.
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate.
        }
    }

    private void Start()
    {
        // Get the camera's viewport bounds
        mainCamera = Camera.main;
        viewportWidth = (int)(mainCamera.aspect * mainCamera.orthographicSize * 2f);
        viewportHeight = (int)(mainCamera.orthographicSize * 2f);
        Invoke("SpawnPowerUp", UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    private GameObject GetPowerUpPrefab(PowerUpType powerUpType)
    {
        // Find and return the Prefab corresponding to a PowerUpType.
        PowerUp powerUp = Array.Find(powerUps, item => item.powerUpType == powerUpType);
        if (powerUp != null)
        {
            return powerUp.powerUpPrefab; // Return Prefab if found.
        }
        else
        {
            return null; // Return null if no Prefab found.
        }
    }
    public PowerUpType GetPowerUpType(string powerUpPrefabName)
    {
        powerUpPrefabName = powerUpPrefabName.Replace("(Clone)", "");
        // Find and return the PowerUpType corresponding to a PowerUpPrefab.
        PowerUp powerUp = Array.Find(powerUps, item => item.powerUpPrefab.name == powerUpPrefabName);
        if (powerUp != null)
        {
            return powerUp.powerUpType; // Return PowerUpType if found.
        }
        else
        {
            return PowerUpType.NoPowerUp; // Return NoPowerUp if no PowerUpType found.
        }
    }
    private void SpawnPowerUp()
    {
        PowerUpType powerUpType;
        float randomValue = UnityEngine.Random.value;
        if (randomValue <= 0.33f)
        {
            powerUpType = PowerUpType.ScoreBoostPowerUp;
        }
        else if (randomValue <= 0.66f)
        {
            powerUpType = PowerUpType.ShieldPowerUp;
        }
        else
        {
            powerUpType = PowerUpType.SpeedUpPowerUp;
        }

        Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(-viewportWidth / 2f, viewportWidth / 2f),
                                            UnityEngine.Random.Range(-viewportHeight / 2f, viewportHeight / 2f));
        GameObject powerUpPrefab = GetPowerUpPrefab(powerUpType);

        if (powerUpPrefab != null)
        {
            GameObject powerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity, this.transform);
            Destroy(powerUp, powerUpLifetime); // Destroy powerUp after a certain time if not eaten
        }

        // Schedule the next PowerUp spawn
        Invoke("SpawnPowerUp", UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval));
    }
}

[System.Serializable]
public class PowerUp
{
    public PowerUpType powerUpType; // Defines the type of Power Up.
    public GameObject powerUpPrefab; // Holds the prefab associated with the Power Up type.
}

public enum PowerUpType
{
    ScoreBoostPowerUp,
    ShieldPowerUp,
    SpeedUpPowerUp,
    NoPowerUp
}