using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The spinning platform to parent spawned hazards to
    public Transform spinningPlatform;

    // List of available hazard patterns
    public HazardPattern[] hazardPatterns;

    public void StartPattern(int patternIndex)
    {
        if (hazardPatterns != null && patternIndex >= 0 && patternIndex < hazardPatterns.Length)
        {
            StartCoroutine(SpawnPattern(hazardPatterns[patternIndex]));
        }
        else
        {
            Debug.LogWarning("Invalid pattern index or no patterns assigned!");
        }
    }

    void Start()
    {
        GetComponent<Spawner>().StartPattern(0); // Start the first pattern
    }


    private IEnumerator SpawnPattern(HazardPattern pattern)
    {
        Debug.Log($"Starting pattern: {pattern.name}");

        foreach (var spawn in pattern.spawnData)
        {
            if (spawn.hazardPrefab == null)
            {
                Debug.LogWarning("A hazard prefab in the pattern is missing!");
                continue;
            }

            // Calculate spawn position
            Vector3 worldSpawnPosition = transform.TransformPoint(spawn.localPosition);
            Debug.Log($"Spawning hazard prefab {spawn.hazardPrefab.name} at {worldSpawnPosition}");

            // Instantiate hazard
            GameObject spawnedObject = Instantiate(spawn.hazardPrefab, worldSpawnPosition, transform.rotation);

            // Parent it to the spinning platform
            if (spinningPlatform != null)
            {
                spawnedObject.transform.SetParent(spinningPlatform, true);
                Debug.Log("Hazard parented to spinning platform.");
            }

            // Set kill time
            float killTime = spawn.useOverrideKillTime ? spawn.overrideKillTime : pattern.defaultKillTime;
            Hazard hazardScript = spawnedObject.GetComponent<Hazard>();
            if (hazardScript != null)
            {
                hazardScript.SetKillTime(killTime);
                Debug.Log($"Set hazard kill time to {killTime}");
            }
            else
            {
                Debug.LogWarning("Hazard prefab does not have a Hazard script attached.");
            }
        }

        yield return null; // Allow the coroutine to finish immediately
    }


}
