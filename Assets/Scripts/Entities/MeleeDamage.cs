using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false; // para saber de donde leer la _weaponData
    private int _damage;

    private void Awake()
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

        if (other.TryGetComponent(out Entity entity))
            entity.TakeDamage(_damage);
    }
}
