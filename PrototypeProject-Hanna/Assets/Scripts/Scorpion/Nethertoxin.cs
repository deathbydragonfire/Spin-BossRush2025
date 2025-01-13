using System.Collections;
using UnityEngine;

public class Nethertoxin : MonoBehaviour
{
    [SerializeField] private Vector2 rangeOfDuration;
    private float duration;
    [SerializeField] private float damage;
    [SerializeField] private float damageRate;
    [SerializeField] private float radius;

    void Start()
    {
        duration = Random.Range(rangeOfDuration.x, rangeOfDuration.y);
        Destroy(gameObject , duration);
        StartCoroutine(Damage());
    }

    private IEnumerator Damage()
    {
        while (true)
        {
            DealDamage();
            yield return new WaitForSeconds(damageRate);
        }
    }


    private void DealDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            Health targetHealth = collider.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamageByCurrentHP(damage);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
