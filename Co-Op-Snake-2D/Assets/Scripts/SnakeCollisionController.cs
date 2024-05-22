using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollisionController : MonoBehaviour
{
    private LayerMask collisionLayer; // This will be fetched from the parent object
    SnakeController snakeController;
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
        if (collider2D.gameObject.layer == collisionLayerIndex)
        {
            // Check if the application is running in the Unity Editor.
            #if UNITY_EDITOR
            {
                // Stop playing the scene in the Unity Editor.
                UnityEditor.EditorApplication.isPlaying = false;
            }
            #else
                Application.Quit();
            #endif
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
    }
}
