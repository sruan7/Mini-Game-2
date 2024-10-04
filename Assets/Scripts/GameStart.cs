using System.Collections; // Required for IEnumerator
using UnityEngine;
using TMPro; // For TextMeshPro

public class GameStart : MonoBehaviour
{
    public TextMeshProUGUI startText; // The "Press Space to Start" text in the middle of the screen
    public GameObject orderPanel; // The square (OrderPanel) to display the order
    public TextMeshProUGUI orderText; // The TextMeshPro component inside the OrderPanel
    public RecipeManager recipeManager; // Reference to the RecipeManager script

    private bool gameStarted = false;

    void Start()
    {
        // Initially hide the order panel
        orderPanel.SetActive(false);
    }

    void Update()
    {
        // If space is pressed and the game hasn't started yet
        if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
        {
            gameStarted = true;

            // Hide the "Press Space to Start" text
            startText.gameObject.SetActive(false);

            // Start the sequence to show the order after 1 second
            StartCoroutine(ShowOrderAfterDelay());
        }
    }

    // Coroutine to wait for 1 second and then show the order
    IEnumerator ShowOrderAfterDelay()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1.0f);

        // Show the order panel (the square on the left)
        orderPanel.SetActive(true);

        // Generate and display the random order
        recipeManager.GenerateRandomRecipe();
    }
}
