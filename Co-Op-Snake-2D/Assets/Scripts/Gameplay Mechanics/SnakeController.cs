using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public int initialBodySize = 1; // Initial number of body segments
    public float speed = 1f; // Speed of the snake
    public float moveStep = 1f; // The distance the snake moves each step
    public float powerUpCoolDownTimer; // The cooldown timer of powerup
    public int scoreBoostPowerUpRatio = 1; // The ratio by which score will increase on Score Boost PowerUp

    [HideInInspector]
    public bool isShieldPowerUpActive = false; // Indicates if the shield power-up is active

    public float speedUpPowerUpRatio = 1.0f; // The increment ratio for speed on Speed Up PowerUp
    public Camera mainCamera; // Reference to the main camera
    public GameObject snakeHeadPrefab; // Prefab for the snake head
    public GameObject snakeBodyPrefab; // Prefab for the snake body
    public GameObject gameOverObject; // Reference to the Game Over object
    public GameObject gamePauseObject; // Reference to the Game Pause object
    public LayerMask collisionLayer; // Layer for collision detection

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
    private int currentScoreBoostRatio = 1; // The increment ratio for Score Boost on PowerUp

    private void Awake()
    {
        snakeSegments = new List<Transform>();
        previousPositions = new List<Vector2>();
    }

    private void Start()
    {
        InitializeBodySegment();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Initialize the snake body with the initial number of segments
    private void InitializeBodySegment()
    {
        GameObject headSegment = Instantiate(snakeHeadPrefab, transform.position, Quaternion.identity, transform);
        snakeSegments.Add(headSegment.transform);
        previousPositions.Add(headSegment.transform.position);
        AddBodySegment(initialBodySize);
    }

    // Add new body segments to the snake
    public void AddBodySegment(int addLength)
    {
        SoundManager.Instance.PlayEffect(SoundType.SnakeHeal);
        for (int i = 0; i < addLength; i++)
        {
            Vector2 newSegmentPosition = (Vector2)snakeSegments[snakeSegments.Count - 1].position - direction * moveStep;
            GameObject bodySegment = Instantiate(snakeBodyPrefab, newSegmentPosition, Quaternion.identity, transform);
            snakeSegments.Add(bodySegment.transform);
            previousPositions.Add(bodySegment.transform.position);
        }
    }

    // Reduce body segments from the snake
    public void ReduceBodySegment(int reduceLength)
    {
        SoundManager.Instance.PlayEffect(SoundType.SnakeHurt);
        for (int i = 0; i < reduceLength; i++)
        {
            GameObject bodySegment = snakeSegments[snakeSegments.Count - 1].gameObject;
            previousPositions.RemoveAt(snakeSegments.Count - 1);
            snakeSegments.RemoveAt(snakeSegments.Count - 1);
            Destroy(bodySegment);
        }
    }

    // Handle user input for controlling the snake
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
            GamePauseController gamePauseController = gamePauseObject.GetComponent<GamePauseController>();
            gamePauseController?.GamePause();
        }
    }

    // Move the snake in the current direction
    private void Move()
    {
        moveTimer += Time.fixedDeltaTime;
        if (moveTimer >= moveStep / (speed * Time.fixedDeltaTime))
        {
            // Move the snake head
            snakeSegments[0].position = (Vector2)snakeSegments[0].position + direction * moveStep;
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

    // Wrap the snake around the screen if it goes out of bounds
    private void WrapAroundScreen(Transform segment)
    {
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(segment.position);
        screenPosition.x = (screenPosition.x < 0) ? 1 : (screenPosition.x > 1) ? 0 : screenPosition.x;
        screenPosition.y = (screenPosition.y < 0) ? 1 : (screenPosition.y > 1) ? 0 : screenPosition.y;
        segment.position = mainCamera.ViewportToWorldPoint(screenPosition);
    }

    // Activate the corresponding power-up
    public IEnumerator ActivatePowerUp(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.ScoreBoostPowerUp:
                yield return ActivateScoreBoostPowerUp();
                break;
            case PowerUpType.ShieldPowerUp:
                yield return ActivateShieldPowerUp();
                break;
            case PowerUpType.SpeedUpPowerUp:
                yield return ActivateSpeedUpPowerUp();
                break;
        }
    }

    // Activate the Score Boost power-up
    private IEnumerator ActivateScoreBoostPowerUp()
    {
        specialAbilityPopUpMessage.text = LayerMask.LayerToName(gameObject.layer) + " activates Score Boost for " + powerUpCoolDownTimer + " seconds.";
        specialAbilityPopUpMessage.gameObject.SetActive(true);
        currentScoreBoostRatio = scoreBoostPowerUpRatio;
        yield return new WaitForSeconds(powerUpCoolDownTimer);
        currentScoreBoostRatio = 1;
        specialAbilityPopUpMessage.gameObject.SetActive(false);
    }

    // Activate the Shield power-up
    private IEnumerator ActivateShieldPowerUp()
    {
        specialAbilityPopUpMessage.text = LayerMask.LayerToName(gameObject.layer) + " activates Shield Boost for " + powerUpCoolDownTimer + " seconds.";
        specialAbilityPopUpMessage.gameObject.SetActive(true);
        isShieldPowerUpActive = true;
        yield return new WaitForSeconds(powerUpCoolDownTimer);
        isShieldPowerUpActive = false;
        specialAbilityPopUpMessage.gameObject.SetActive(false);
    }

    // Activate the Speed Up power-up
    private IEnumerator ActivateSpeedUpPowerUp()
    {
        specialAbilityPopUpMessage.text = LayerMask.LayerToName(gameObject.layer) + " activates Speed Up for " + powerUpCoolDownTimer + " seconds.";
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
        if (!isShieldPowerUpActive)
        {
            score -= decrementScore;
            score = Math.Max(score, 0);
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        scoreText.text = "Score " + LayerMask.LayerToName(gameObject.layer) + ": " + score;
    }

    public IEnumerator Die()
    {
        GameOverController gameOverController = gameOverObject.GetComponent<GameOverController>();
        gameOverController?.UpdateWinner();
        yield return new WaitForSeconds(0.1f);
        gameOverController?.GameOver();
        gameObject.SetActive(false);
    }
}
