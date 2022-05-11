using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
            other.collider.GetComponent<Enemy>().TakeDamage(10);
    }
}
