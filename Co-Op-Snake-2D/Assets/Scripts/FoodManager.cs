using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private static FoodManager instance;
    public static FoodManager Instance { get { return instance; } } // Singleton instance accessor.

    private Camera mainCamera;
    private int viewportWidth;
    private int viewportHeight;

    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 6f;
    public float foodLifetime = 10f;
    public Food[] foods; // Array of food configurations.

    private void Awake()
    {
        // Ensure only one instance of FoodManager exists.
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
        Invoke("SpawnFood", UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    private GameObject GetFoodPrefab(FoodType foodType)
    {
        // Find and return the Prefab corresponding to a FoodType.
        Food food = Array.Find(foods, item => item.foodType == foodType);
        if (food != null)
        {
            return food.foodPrefab; // Return Prefab if found.
        }
        else
        {
            return null; // Return null if no Prefab found.
        }
    }
    public int GetFoodAffectedLength(FoodType foodType)
    {
        // Find and return the Affected Length corresponding to a FoodType.
        Food food = Array.Find(foods, item => item.foodType == foodType);
        if (food != null)
        {
            return food.foodAffectedLength; // Return Affected Length if found.
        }
        else
        {
            return 0; // Return 0 if no FoodType found.
        }
    }
    public int GetFoodAffectedScore(FoodType foodType)
    {
        // Find and return the Affected Score corresponding to a FoodType.
        Food food = Array.Find(foods, item => item.foodType == foodType);
        if (food != null)
        {
            return food.foodAffectedScore; // Return Affected Score if found.
        }
        else
        {
            return 0; // Return 0 if no FoodType found.
        }
    }
    public FoodType GetFoodType(string foodPrefabName)
    {
        foodPrefabName = foodPrefabName.Replace("(Clone)", "");
        // Find and return the FoodType corresponding to a FoodPrefab.
        Food food = Array.Find(foods, item => item.foodPrefab.name == foodPrefabName);
        if (food != null)
        {
            return food.foodType; // Return FoodType if found.
        }
        else
        {
            return FoodType.NoFood; // Return NoFood if no FoodType found.
        }
    }
    private void SpawnFood()
    {
        FoodType foodType = (UnityEngine.Random.value > 0.5f) ? FoodType.MassGainer : FoodType.MassBurner;

        Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(-viewportWidth / 2f, viewportWidth / 2f),
                                            UnityEngine.Random.Range(-viewportHeight / 2f, viewportHeight / 2f));
        GameObject foodPrefab = GetFoodPrefab(foodType);

        if (foodPrefab != null)
        {
            GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity, this.transform);
            Destroy(food, foodLifetime); // Destroy food after a certain time if not eaten
        }

        // Schedule the next food spawn
        Invoke("SpawnFood", UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval));
    }
}

[System.Serializable]
public class Food
{
    public FoodType foodType; // Defines the type of food.
    public GameObject foodPrefab; // Holds the prefab associated with the food type.
    public int foodAffectedLength = 0;
    public int foodAffectedScore = 0;
}

public enum FoodType
{
    MassGainer,
    MassBurner,
    NoFood
}