using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false; // para saber de donde leer la _weaponData
    private int _damage;

    private void Start()
    {
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
    }

    private void ReflectBullet(Rigidbody rb)
    {
        // de momento esta implementacion no causa
        // ningun problema
        rb.velocity = -rb.velocity * 2;
    }
}