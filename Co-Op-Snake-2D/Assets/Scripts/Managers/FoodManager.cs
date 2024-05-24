using System;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private static FoodManager instance;
    public static FoodManager Instance { get { return instance; } }

    private Camera mainCamera;
    private int viewportWidth;
    private int viewportHeight;

    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 6f;
    public float foodLifetime = 10f;
    public Food[] foods;

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
        Invoke("SpawnFood", UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    private GameObject GetFoodPrefab(FoodType foodType)
    {
        Food food = Array.Find(foods, item => item.foodType == foodType);
        return food?.foodPrefab;
    }

    public int GetFoodAffectedLength(FoodType foodType)
    {
        Food food = Array.Find(foods, item => item.foodType == foodType);
        return food?.foodAffectedLength ?? 0;
    }

    public int GetFoodAffectedScore(FoodType foodType)
    {
        Food food = Array.Find(foods, item => item.foodType == foodType);
        return food?.foodAffectedScore ?? 0;
    }

    public FoodType GetFoodType(string foodPrefabName)
    {
        foodPrefabName = foodPrefabName.Replace("(Clone)", "");
        Food food = Array.Find(foods, item => item.foodPrefab.name == foodPrefabName);
        return food?.foodType ?? FoodType.NoFood;
    }

    private void SpawnFood()
    {
        FoodType foodType = UnityEngine.Random.value > 0.5f ? FoodType.MassGainer : FoodType.MassBurner;
        Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(-viewportWidth / 2f, viewportWidth / 2f),
                                            UnityEngine.Random.Range(-viewportHeight / 2f, viewportHeight / 2f));
        GameObject foodPrefab = GetFoodPrefab(foodType);

        if (foodPrefab != null)
        {
            GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity, transform);
            Destroy(food, foodLifetime);
        }

        Invoke("SpawnFood", UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval));
    }
}

[Serializable]
public class Food
{
    public FoodType foodType;
    public GameObject foodPrefab;
    public int foodAffectedLength = 0;
    public int foodAffectedScore = 0;
}

public enum FoodType
{
    MassGainer,
    MassBurner,
    NoFood
}
