using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public Scene gameScene;

    public void ContinueGame()
    {
        SceneManager.LoadScene(gameScene.name); // Reload the current level
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
