using UnityEngine;

[CreateAssetMenu(fileName = "NewHazardPattern", menuName = "Hazard Pattern")]
public class HazardPattern : ScriptableObject
{
    [System.Serializable]
    public class SpawnData
    {
        public GameObject hazardPrefab; // Hazard prefab to spawn
        public Vector3 localPosition;   // Position relative to the spawner

        [SerializeField] public bool useOverrideDelay = false;
        [SerializeField] public float overrideDelay = 0.5f;

        [SerializeField] public bool useOverrideKillTime = false;
        [SerializeField] public float overrideKillTime = 5f;
    }

    [SerializeField] public float defaultDelay = 0.5f; // Default delay between spawns
    [SerializeField] public float defaultKillTime = 5f; // Default time before hazards are destroyed
    [SerializeField] public SpawnData[] spawnData;     // Array of spawn instructions
}
