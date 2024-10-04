using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Move from StartScene to InstructionScene
            if (SceneManager.GetActiveScene().name == "StartScene")
            {
                SceneManager.LoadScene("InstructionScene");
                SceneManager.UnloadSceneAsync("StartScene");
            }
            // Move from InstructionScene to MainScene
            else if (SceneManager.GetActiveScene().name == "InstructionScene")
            {
                SceneManager.LoadScene("MainScene");
                SceneManager.UnloadSceneAsync("InstructionScene");
            }
        }
    }
}


