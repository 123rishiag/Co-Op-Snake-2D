using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollisionController : MonoBehaviour
{
    private LayerMask collisionLayer; // This will be fetched from the parent object
    private SnakeController snakeController;
    private void Start()
    {
        snakeController = GetComponentInParent<SnakeController>();
        if (snakeController != null)
        {
            collisionLayer = snakeController.collisionLayer;
        }
        else
        {
            Debug.LogError("SnakeController script not found on the parent object.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        int collisionLayerIndex = (int)Mathf.Log(collisionLayer.value, 2);
        // Check for self-collision using layers and tags
        if (collider2D.gameObject.layer == collisionLayerIndex && snakeController.isShieldPowerUpActive == false)
        {
            StartCoroutine(snakeController.Die());
        }
        else if (LayerMask.LayerToName(collider2D.gameObject.layer) == "Food")
        {
            FoodType foodType = FoodManager.Instance.GetFoodType(collider2D.gameObject.name);
            if (foodType == FoodType.MassGainer)
            {
                snakeController.AddBodySegment(FoodManager.Instance.GetFoodAffectedLength(foodType));
                snakeController.IncreaseScore(FoodManager.Instance.GetFoodAffectedScore(foodType));
            }
            else if (foodType == FoodType.MassBurner && snakeController.GetSnakeBodyLength() > FoodManager.Instance.GetFoodAffectedLength(foodType))
            {
                snakeController.ReduceBodySegment(FoodManager.Instance.GetFoodAffectedLength(foodType));
                snakeController.DecreaseScore(FoodManager.Instance.GetFoodAffectedScore(foodType));
            }
            Destroy(collider2D.gameObject);
        }
        else if (LayerMask.LayerToName(collider2D.gameObject.layer) == "PowerUp")
        {
            PowerUpType powerUpType = PowerUpManager.Instance.GetPowerUpType(collider2D.gameObject.name);
            if (powerUpType == PowerUpType.ScoreBoostPowerUp)
            {
                StartCoroutine(snakeController.ActivateScoreBoostPowerUp());
            }
            else if (powerUpType == PowerUpType.ShieldPowerUp)
            {
                StartCoroutine(snakeController.ActivateShieldPowerUp());
            }
            else if (powerUpType == PowerUpType.SpeedUpPowerUp)
            {
                StartCoroutine(snakeController.ActivateSpeedUpPowerUp());
            }
            Destroy(collider2D.gameObject);
        }
    }
}
