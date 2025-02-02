using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "GameScene"; // Set this to your actual game scene name in Unity

    public void StartGame()
    {
        Debug.Log("Start button pressed! Loading game...");
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed! Exiting game...");
        Application.Quit();
    }
}
