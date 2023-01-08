using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatakWall : MonoBehaviour
{
    public GameObject buttonPrefab;

    // The grid of buttons
    public GameObject[,] buttonGrid;

    // The time limit for the game
    public float timeLimit = 10f;

    // The current time remaining in the game
    private float timeRemaining;

    // The score of the player
    private int score;

    // A flag indicating whether the game is over
    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the time remaining and score
        timeRemaining = timeLimit;
        score = 0;

        // Initialize the button grid
        buttonGrid = new GameObject[5, 5];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                // Instantiate a button and add it to the grid
                GameObject button = Instantiate(buttonPrefab, transform);
                button.transform.position = new Vector3(i, j, 0);
                buttonGrid[i, j] = button;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is over
        if (gameOver)
        {
            return;
        }

        // Decrement the time remaining
        timeRemaining -= Time.deltaTime;

        // Check if the time limit has been reached
        if (timeRemaining <= 0f)
        {
            // End the game
            EndGame();
        }
    }

    // A function to handle button clicks
    public void ButtonClicked(GameObject button)
    {
        // Increase the score
        score++;

        // Deactivate the button to prevent further clicks
        button.SetActive(false);
    }

    // A function to end the game
    private void EndGame()
    {
        // Set the game over flag to true
        gameOver = true;

        // Display the score and time remaining
        Debug.Log("Game over! Score: " + score + " Time remaining: " + timeRemaining);
    }
}
