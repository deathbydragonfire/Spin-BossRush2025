using UnityEngine;
using System.Collections;
public class PlayerAttack : MonoBehaviour
{
    //public float attackDamage = 10f;
    public GameObject attackHitbox; // Assign this in the Inspector

    private void Start()
    {
        attackHitbox.SetActive(false); // Make sure it's OFF by default
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to attack
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        attackHitbox.SetActive(true); // Activate hitbox
        yield return new WaitForSeconds(0.2f); // Hitbox stays active for a short time
        attackHitbox.SetActive(false); // Disable hitbox after attack
    }

}
