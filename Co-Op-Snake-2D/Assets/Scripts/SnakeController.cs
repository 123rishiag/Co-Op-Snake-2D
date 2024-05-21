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

    private GameObject snakeHead; // Object for the snake head segment
    private GameObject snakeBody; // Object for the snake body segment
    private Vector2 direction = Vector2.left; // Initial direction of movement
    private List<Transform> snakeSegments; // List to hold snake segments
    private List<Vector2> previousPositions; // List to hold previous positions of the snake segments

    private float moveTimer;

    private void Awake()
    {
        snakeHead = transform.Find("Head").gameObject;
        snakeBody = snakeHead.transform.Find("Body").gameObject;
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
        Vector2 newSegmentPosition;
        snakeSegments.Add(snakeHead.transform);
        previousPositions.Add(snakeHead.transform.position);
        snakeSegments.Add(snakeBody.transform);
        previousPositions.Add(snakeBody.transform.position);
        newSegmentPosition = snakeBody.transform.position;
        if (initialBodySize == 0)
        {
            AddBodySegment(0);
        }
        else
        {
            AddBodySegment(initialBodySize - 1);
        }
    }
    private void AddBodySegment(int addLength)
    {
        Vector2 newSegmentPosition;
        for (int i = 0; i < addLength; i++)
        {
            // Calculate the initial position of the new body segment
            newSegmentPosition = (Vector2)snakeSegments[snakeSegments.Count - 1].position - direction * moveStep;
            // Instantiate and add the new body segment
            GameObject bodySegment = Instantiate(snakeBody, newSegmentPosition, Quaternion.identity);
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
            // Store the current head position
            Vector2 previousPosition = snakeHead.transform.position;

            // Move the snake head
            snakeHead.transform.position = previousPosition + direction * moveStep;

            // Wrap the snake head if it goes beyond the screen bounds
            WrapAroundScreen(snakeHead.transform);

            // Update previous positions list
            previousPositions.Insert(0, snakeHead.transform.position);
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
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(snakeHead.transform.position);

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

        snakeHead.transform.position = mainCamera.ViewportToWorldPoint(screenPosition);
    }
}
