using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathScreenHandler : MonoBehaviour
{
    public string scene;

    public void ContinueGame()
    {
        Debug.Log("clicked continue");
        SceneManager.LoadScene(scene); // Reload the current level
    }

    public void ExitGame()
    {
        Debug.Log("clicked exit");
        Application.Quit();
    }
}