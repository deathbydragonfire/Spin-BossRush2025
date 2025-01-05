using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The spinning platform to parent spawned objects to
    public Transform spinningPlatform;

    // Prefab to spawn
    public GameObject objectToSpawn;

    // Spawn interval in seconds
    public float spawnInterval = 2f;

    // Random X offset range (local X-axis)
    public float minXOffset = -1f;
    public float maxXOffset = 1f;

    // Internal timer
    private float timer;

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to spawn a new object
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f; // Reset the timer
        }
    }

    void SpawnObject()
    {
        if (objectToSpawn != null && spinningPlatform != null)
        {
            // Generate a random X offset (local space)
            float randomXOffset = Random.Range(minXOffset, maxXOffset);

            // Calculate the spawn position with the offset in local space
            Vector3 localSpawnPosition = new Vector3(randomXOffset, 0, 0);
            Vector3 worldSpawnPosition = transform.TransformPoint(localSpawnPosition);

            // Instantiate the prefab at the calculated position and the spawner's rotation
            GameObject spawnedObject = Instantiate(objectToSpawn, worldSpawnPosition, transform.rotation);

            // Parent the spawned object to the spinning platform
            spawnedObject.transform.SetParent(spinningPlatform, true);
        }
        else
        {
            Debug.LogWarning("Spinning platform or object to spawn is not assigned!");
        }
    }
}
