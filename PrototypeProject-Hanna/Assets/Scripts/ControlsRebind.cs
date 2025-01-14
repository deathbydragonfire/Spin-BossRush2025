
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this at the top for TextMeshPro support


public class ControlsRebind : MonoBehaviour
{
    public TMP_Text actionText; // Use TMP_Text instead of Text
    private KeyCode currentKey = KeyCode.W; // Default keybinding

    public void StartRebinding()
    {
        StartCoroutine(WaitForKeyPress());
    }

    private IEnumerator WaitForKeyPress()
    {
        actionText.text = "Press a key...";
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                currentKey = key;
                actionText.text = $"Current Key: {key}";
                Debug.Log($"Key rebound to: {key}");
                break;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(currentKey))
        {
            Debug.Log($"Action triggered by: {currentKey}");
        }
    }
}
