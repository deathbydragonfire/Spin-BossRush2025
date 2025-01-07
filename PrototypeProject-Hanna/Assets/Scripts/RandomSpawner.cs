using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // Reference to the Spawner component
    public Spawner spawner;

    // Interval between random pattern spawns (in seconds)
    public float spawnInterval = 5f;

    void Start()
    {
        // Start spawning random patterns at regular intervals
        InvokeRepeating(nameof(SpawnRandomPattern), 0f, spawnInterval);
    }

    void SpawnRandomPattern()
    {
        if (spawner == null || spawner.hazardPatterns == null || spawner.hazardPatterns.Length == 0)
        {
            Debug.LogWarning("Spawner or hazard patterns not set up correctly!");
            return;
        }

        // Select a random pattern index
        int randomIndex = Random.Range(0, spawner.hazardPatterns.Length);

        // Spawn the selected random pattern
        spawner.StartPattern(randomIndex);
    }
}
