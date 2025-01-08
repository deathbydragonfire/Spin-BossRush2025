using UnityEngine;

public class PyramidHead : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject flyAreaPrefab;
    [SerializeField] private Transform flyArea;
    private Vector3 destination;
    void Start()
    {
        flyArea = Instantiate(flyAreaPrefab).transform;
        flyArea.parent = null;
        destination = GetNextPos();
    }

    void Update()
    {
        transform.Rotate(0f ,rotationSpeed * Time.deltaTime, 0f);
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position , destination) < 0.05f) destination = GetNextPos();
    }

    private Vector3 GetNextPos()
    {
        float x = Random.Range(-flyArea.localScale.x/2 , +flyArea.localScale.x/2) + flyArea.position.x;
        float y = Random.Range(-flyArea.localScale.y/2 , +flyArea.localScale.y/2) + flyArea.position.y;
        float z = Random.Range(-flyArea.localScale.z/2 , +flyArea.localScale.z/2) + flyArea.position.z;
        return new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(flyArea.position , flyArea.localScale);
    }

    private void OnDestroy()
    {
        Destroy(flyArea.gameObject);
    }
}