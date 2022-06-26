using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false; // para saber de donde leer la _weaponData
    [SerializeField] private int _pushForce = 10; // este lo puse solo para impulsar a los pickables
    private CameraBehaviour _cameraBehaviour;

    // si se tiene que usar para empujar a los enemigos tambien, pasarlo al weapondata.cs
    private Transform _transform;
    private int _damage;
    private float _shakeTime;
    private float _shakeForce;

    private void Start()
    {
        _cameraBehaviour = GameManager.GetInstance.GetCameraBehaviour;

        _transform = GetComponent<Transform>();
        // le asigna el valor al daño del melee
        // en funcion de lo que esta en el scriptabledc
        int weaponToGet = 0;
        WeaponData weaponData;

        if (_isPlayer)
            weaponToGet = (int)WeaponData.Weapons.Bat;

        weaponData = GetComponentInParent<Entity>().GetWeaponList[weaponToGet];

        _damage = weaponData.MeleeDamage;
        _shakeTime = weaponData.ShakeTime;
        _shakeForce = weaponData.ShootShake;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool shake = false;
        // como las hitbox del player estan en un padre
        // hay que hacer algo un poco distinto
        if (other.CompareTag("Player"))
            other.GetComponentInParent<Player>().TakeDamage(_damage);

        if (other.TryGetComponent(out Enemy enemy))
        {
            shake = true;
            enemy.SetMeleeDamage = true;
            enemy.TakeDamage(_damage);

            Rigidbody rb = other.GetComponentInChildren<Rigidbody>();

            if (!rb) return;
            Vector2 dir = (other.transform.position - _transform.position).normalized;

            rb.AddForce(dir * _pushForce * 50, ForceMode.Impulse);
        }

        if (other.CompareTag("Bullet"))
        {
            shake = true;
            ReflectBullet(other.GetComponent<Rigidbody>());
        }

        if (other.CompareTag("Ammo") || other.CompareTag("Health"))
        {
            shake = true;
            Vector2 otherPos = other.transform.position;
            Rigidbody otherRb;

            if (other.TryGetComponent(out Rigidbody rb))
            {
                otherRb = rb;
                Vector2 direction = otherPos - (Vector2)_transform.position;

                rb.AddForce(direction.normalized * _pushForce, ForceMode.Impulse);
            }
        }

        if (other.CompareTag("Barrel"))
            shake = true;

        if (shake && _isPlayer)
        {
            _cameraBehaviour.ShakeCamera(_shakeForce, _shakeTime);
            shake = false;
        }
    }

    private void ReflectBullet(Rigidbody rb)
    {
        // de momento esta implementacion no causa
        // ningun problema
        rb.velocity = -rb.velocity * 2;
    }
}