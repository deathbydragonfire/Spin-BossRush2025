using UnityEngine;

public class NethertoxinProjectile : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float time;
    [SerializeField] private LayerMask mask;
    [SerializeField] private GameObject nethertoxinPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(ShootProjectileForce(FindFirstObjectByType<PlayerController3D>().transform.position , Random.Range((time*2/3) , (time*3/2))), ForceMode.VelocityChange);
    }

    private Vector3 ShootProjectileForce(Vector3 targetPosition, float time)
    {
        Vector3 startPosition = transform.position;
        Vector3 horizontalDisplacement = new Vector3(
            targetPosition.x - startPosition.x,
            0,
            targetPosition.z - startPosition.z
        );
        float verticalDisplacement = targetPosition.y - startPosition.y;
        float gravity = Physics.gravity.y;

        Vector3 horizontalVelocity = horizontalDisplacement / time;

        float verticalVelocity = (verticalDisplacement - 0.5f * gravity * time * time) / time;

        Vector3 force = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

        return force;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((mask.value & (1 << other.gameObject.layer)) != 0)
        {
            var obj = Instantiate(nethertoxinPrefab , other.ClosestPoint(transform.position) , Quaternion.identity);
            obj.transform.parent = other.transform;
            Destroy(gameObject);
        }
    }
}
