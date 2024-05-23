using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public int initialBodySize = 1; // Initial number of body segments
    public float speed = 1f; // Speed of the snake
    public float moveStep = 1f; // The distance the snake moves each step
    public float powerUpCoolDownTimer; // The cooldown Timer of powerup
    public int scoreBoostPowerUpRatio = 1; // The ratio by which Score will increase on Score Boost PowerUp

    [HideInInspector]
    public bool isShieldPowerUpActive = false; // The flag which denotes shield state on Shield PowerUp

    public float speedUpPowerUpRatio = 1.0f; // The increment ratio by which Speed will increase on Speed Up PowerUp
    public Camera mainCamera; // Reference to the main camera
    public GameObject snakeHeadPrefab; // Object for the snake head Prefab
    public GameObject snakeBodyPrefab; // Object for the snake body Prefab
    public GameObject gameOverObject; // Object for the Game Over
    public GameObject gamePauseObject; // Object for the Game Over
    public LayerMask collisionLayer; // Layer which the snake can collide with

    private Vector2 direction = Vector2.left; // Initial direction of movement
    private List<Transform> snakeSegments; // List to hold snake segments
    private List<Vector2> previousPositions; // List to hold previous positions of the snake segments

    private float moveTimer;

    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode moveLeft;
    public KeyCode moveRight;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI specialAbilityPopUpMessage;
    private int score = 0;
    private int currentScoreBoostRatio = 1;
    

    private void Awake()
    {
        snakeSegments = new List<Transform>();
        previousPositions = new List<Vector2>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitializeBodySegment();
    }

    private void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void InitializeBodySegment()
    {
        GameObject headSegment = Instantiate(snakeHeadPrefab, this.transform.position, Quaternion.identity, this.transform);
        snakeSegments.Add(headSegment.transform);
        previousPositions.Add(headSegment.transform.position);
        AddBodySegment(initialBodySize);
    }
    public void AddBodySegment(int addLength)
    {
        SoundManager.Instance.PlayEffect(SoundType.SnakeHeal);
        Vector2 newSegmentPosition;
        for (int i = 0; i < addLength; i++)
        {
            // Calculate the initial position of the new body segment
            newSegmentPosition = (Vector2)snakeSegments[snakeSegments.Count - 1].position - direction * moveStep;
            // Instantiate and add the new body segment
            GameObject bodySegment = Instantiate(snakeBodyPrefab, newSegmentPosition, Quaternion.identity, this.transform);
            snakeSegments.Add(bodySegment.transform);
            previousPositions.Add(bodySegment.transform.position);
        }
    }
    public void ReduceBodySegment(int reduceLength)
    {
        SoundManager.Instance.PlayEffect(SoundType.SnakeHurt);
        for (int i = 0; i < reduceLength; i++)
        {
            // Drop the last body segment
            GameObject bodySegment = snakeSegments[snakeSegments.Count - 1].gameObject;
            previousPositions.RemoveAt(snakeSegments.Count - 1);
            snakeSegments.RemoveAt(snakeSegments.Count - 1);
            Destroy(bodySegment);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(moveUp) && direction != Vector2.down)
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(moveDown) && direction != Vector2.up)
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(moveLeft) && direction != Vector2.right)
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(moveRight) && direction != Vector2.left)
        {
            direction = Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Notify the Game Pause Controller to check if all snakes are dead
            GamePauseController gamePauseController = gamePauseObject.GetComponent<GamePauseController>();

            if (gamePauseController != null)
            {
                gamePauseController.GamePause();
            }
        }
    }

    private void Move()
    {
        moveTimer += Time.fixedDeltaTime;

        if (moveTimer >= moveStep / (speed * Time.fixedDeltaTime))
        {
            // Move the snake head
            snakeSegments[0].position = (Vector2)snakeSegments[0].position + direction * moveStep;

            // Wrap the snake head if it goes beyond the screen bounds
            WrapAroundScreen(snakeSegments[0]);

            // Update previous positions list
            previousPositions.Insert(0, snakeSegments[0].position);
            previousPositions.RemoveAt(previousPositions.Count - 1);

            // Move each body segment to the position of the segment in front of it
            for (int i = 1; i < snakeSegments.Count; i++)
            {
                snakeSegments[i].position = previousPositions[i];
            }
            moveTimer = 0f;
        }
    }

    private void WrapAroundScreen(Transform segment)
    {
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(segment.position);

        if (screenPosition.x < 0)
        {
            screenPosition.x = 1;
        }
        else if (screenPosition.x > 1)
        {
            screenPosition.x = 0;
        }

        if (screenPosition.y < 0)
        {
            screenPosition.y = 1;
        }
        else if (screenPosition.y > 1)
        {
            screenPosition.y = 0;
        }

        segment.position = mainCamera.ViewportToWorldPoint(screenPosition);
    }
    public IEnumerator ActivateScoreBoostPowerUp()
    {
        specialAbilityPopUpMessage.text = LayerMask.LayerToName(this.gameObject.layer) + " activates Score Boost for " + powerUpCoolDownTimer + " seconds.";
        specialAbilityPopUpMessage.gameObject.SetActive(true);
        currentScoreBoostRatio = scoreBoostPowerUpRatio;
        yield return new WaitForSeconds(powerUpCoolDownTimer);
        currentScoreBoostRatio = 1;
        specialAbilityPopUpMessage.gameObject.SetActive(false);
    }
    public IEnumerator ActivateShieldPowerUp()
    {
        specialAbilityPopUpMessage.text = LayerMask.LayerToName(this.gameObject.layer) + " activates Shield Boost for " + powerUpCoolDownTimer + " seconds.";
        specialAbilityPopUpMessage.gameObject.SetActive(true);
        isShieldPowerUpActive = true;
        yield return new WaitForSeconds(powerUpCoolDownTimer);
        isShieldPowerUpActive = false;
        specialAbilityPopUpMessage.gameObject.SetActive(false);
    }
    public IEnumerator ActivateSpeedUpPowerUp()
    {
        specialAbilityPopUpMessage.text = LayerMask.LayerToName(this.gameObject.layer) + " activates Speed Up for " + powerUpCoolDownTimer + " seconds.";
        specialAbilityPopUpMessage.gameObject.SetActive(true);
        speed *= speedUpPowerUpRatio;
        yield return new WaitForSeconds(powerUpCoolDownTimer);
        speed /= speedUpPowerUpRatio;
        specialAbilityPopUpMessage.gameObject.SetActive(false);
    }
    
    public int GetSnakeBodyLength()
    {
        return snakeSegments.Count - 1;
    }
    public int GetScore()
    {
        return score;
    }
    public void IncreaseScore(int incrementScore)
    {
        score += incrementScore * currentScoreBoostRatio;
        RefreshUI();
    }
    public void DecreaseScore(int decrementScore)
    {
        if (isShieldPowerUpActive == false)
        {
            score -= decrementScore;
            if (score < 0)
            {
                score = 0;
            }
            RefreshUI();
        }
    }
    private void RefreshUI()
    {
        scoreText.text = "Score " + LayerMask.LayerToName(this.gameObject.layer)+ ": " + score;
    }
    public IEnumerator Die()
    {
        // Notify the Game Over Controller to check if all snakes are dead
        GameOverController gameOverController = gameOverObject.GetComponent<GameOverController>();
        
        if (gameOverController != null)
        {
            gameOverController.UpdateWinner();
            yield return new WaitForSeconds(0.1f);
            gameOverController.GameOver();
            // Deactivate the snake
            this.gameObject.SetActive(false);
        }
    }
}