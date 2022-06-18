using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false; // para saber de donde leer la _weaponData
    [SerializeField] private int _pushForce = 10; // este lo puse solo para impulsar a los pickables
    // si se tiene que usar para empujar a los enemigos tambien, pasarlo al weapondata.cs
    private Transform _transform;
    private int _damage;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        // le asigna el valor al daño del melee
        // en funcion de lo que esta en el scriptable
        int weaponToGet = 0;
        WeaponData weaponData;

        if (_isPlayer)
            weaponToGet = (int)WeaponData.Weapons.Bat;

        weaponData = GetComponentInParent<Entity>().GetWeaponList[weaponToGet];

        _damage = weaponData.MeleeDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        // como las hitbox del player estan en un padre
        // hay que hacer algo un poco distinto
        if (other.CompareTag("Player"))
            other.GetComponentInParent<Player>().TakeDamage(_damage);

        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.SetMeleeDamage = true;
            enemy.TakeDamage(_damage);
        }

        if (other.CompareTag("Bullet"))
            ReflectBullet(other.GetComponent<Rigidbody>());

        if (other.CompareTag("Ammo") || other.CompareTag("Health"))
        {
            Vector2 otherPos = other.transform.position;
            Rigidbody otherRb;

            if (other.TryGetComponent(out Rigidbody rb))
            {
                otherRb = rb;
                Vector2 direction = otherPos - (Vector2)_transform.position;

                rb.AddForce(direction.normalized * _pushForce, ForceMode.Impulse);
            }
        }
    }

    private void ReflectBullet(Rigidbody rb)
    {
        // de momento esta implementacion no causa
        // ningun problema
        rb.velocity = -rb.velocity * 2;
    }
}