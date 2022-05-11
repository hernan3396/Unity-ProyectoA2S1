using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.collider.CompareTag("Enemy"))
    //         other.collider.GetComponent<Enemy>().TakeDamage(10);
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            other.GetComponent<Enemy>().TakeDamage(10);
    }
}
