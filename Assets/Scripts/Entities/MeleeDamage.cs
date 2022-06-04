using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // como las hitbox del player estan en un padre
        // hay que hacer algo un poco distinto
        if (other.CompareTag("Player"))
            other.GetComponentInParent<Player>().TakeDamage(10);

        if (other.TryGetComponent(out Entity entity))
            {
                entity.TakeDamage(10);
                other.gameObject.GetComponent<Enemy>().SetMeleeDamage = true;
            }
    }
}
