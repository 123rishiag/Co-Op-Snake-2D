using UnityEngine;

public class SnakeCollisionController : MonoBehaviour
{
    private LayerMask collisionLayer;
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
        if (collider2D.gameObject.layer == collisionLayerIndex && !snakeController.isShieldPowerUpActive)
        {
            StartCoroutine(snakeController.Die());
        }
        else
        {
            HandleCollision(collider2D);
        }
    }

    private void HandleCollision(Collider2D collider2D)
    {
        switch (LayerMask.LayerToName(collider2D.gameObject.layer))
        {
            case "Food":
                HandleFoodCollision(collider2D);
                break;
            case "PowerUp":
                HandlePowerUpCollision(collider2D);
                break;
        }
    }

    private void HandleFoodCollision(Collider2D collider2D)
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

    private void HandlePowerUpCollision(Collider2D collider2D)
    {
        SoundManager.Instance.PlayEffect(SoundType.SpecialAbilityPickup);
        PowerUpType powerUpType = PowerUpManager.Instance.GetPowerUpType(collider2D.gameObject.name);
        StartCoroutine(snakeController.ActivatePowerUp(powerUpType));
        Destroy(collider2D.gameObject);
    }
}
