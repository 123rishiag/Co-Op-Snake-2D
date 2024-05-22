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
    public Camera mainCamera; // Reference to the main camera

    public GameObject snakeHeadPrefab; // Object for the snake head Prefab
    public GameObject snakeBodyPrefab; // Object for the snake body Prefab
    private Vector2 direction = Vector2.left; // Initial direction of movement
    private List<Transform> snakeSegments; // List to hold snake segments
    private List<Vector2> previousPositions; // List to hold previous positions of the snake segments

    private float moveTimer;

    private void Awake()
    {
        snakeSegments = new List<Transform>();
        previousPositions = new List<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeBodySegment();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void InitializeBodySegment()
    {
        GameObject headSegment = Instantiate(snakeHeadPrefab, snakeHeadPrefab.transform.position, Quaternion.identity);
        snakeSegments.Add(headSegment.transform);
        previousPositions.Add(headSegment.transform.position);
        AddBodySegment(initialBodySize);
    }
    private void AddBodySegment(int addLength)
    {
        Vector2 newSegmentPosition;
        for (int i = 0; i < addLength; i++)
        {
            // Calculate the initial position of the new body segment
            newSegmentPosition = (Vector2)snakeSegments[snakeSegments.Count - 1].position - direction * moveStep;
            // Instantiate and add the new body segment
            GameObject bodySegment = Instantiate(snakeBodyPrefab, newSegmentPosition, Quaternion.identity);
            snakeSegments.Add(bodySegment.transform);
            previousPositions.Add(bodySegment.transform.position);
        }
    }


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
        {
            direction = Vector2.right;
        }
    }

    private void Move()
    {
        moveTimer += Time.fixedDeltaTime;

        if (moveTimer >= moveStep / speed)
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
}
