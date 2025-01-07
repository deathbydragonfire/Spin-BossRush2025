using UnityEngine;

[CreateAssetMenu(fileName = "NewHazardPattern", menuName = "Hazard Pattern")]
public class HazardPattern : ScriptableObject
{
    [System.Serializable]
    public class SpawnData
    {
        public GameObject hazardPrefab; // Hazard prefab to spawn
        public Vector3 localPosition;   // Position relative to the spawner
        public float? overrideDelay;    // Optional override for delay
        public float? overrideKillTime; // Optional override for kill time
    }

    public float defaultDelay = 0.5f; // Default delay between spawns
    public float defaultKillTime = 5f; // Default time before hazards are destroyed
    public SpawnData[] spawnData;     // Array of spawn instructions
}
