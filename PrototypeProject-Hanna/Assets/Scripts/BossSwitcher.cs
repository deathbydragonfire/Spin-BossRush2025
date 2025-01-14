using UnityEngine;

public class BossSwitcher : MonoBehaviour
{
    public GameObject[] bosses; // Assign boss GameObjects in the inspector
    private int currentBossIndex = 0;

    private void Start()
    {
        // Activate the first boss if available
        if (bosses.Length > 0 && bosses[currentBossIndex] != null)
        {
            bosses[currentBossIndex].SetActive(true);
        }
    }

    private void SwitchBoss()
    {
        // Deactivate the current boss
        if (bosses[currentBossIndex] != null)
        {
            bosses[currentBossIndex].SetActive(false);
        }

        // Move to the next boss
        currentBossIndex = (currentBossIndex + 1) % bosses.Length;

        // Activate the new boss
        if (bosses[currentBossIndex] != null)
        {
            bosses[currentBossIndex].SetActive(true);
        }
    }
}
