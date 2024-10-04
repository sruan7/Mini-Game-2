using UnityEngine;
using UnityEngine.SceneManagement; // Required to manage scene transitions

public class StartGame : MonoBehaviour
{
    void Update()
    {
        // Check if the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Load the MainScene when space bar is pressed
            SceneManager.LoadScene("MainScene"); // Ensure "MainScene" is the exact name of your scene
        }
    }
}

