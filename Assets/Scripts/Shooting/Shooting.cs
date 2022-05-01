using UnityEngine;

// lo intente hacer medio generico
// para que todo lo que dispares 
// use solo este script llamando
// al metodo Shoot;
public class Shooting : MonoBehaviour
{
    private PoolManager _bulletPool;

    private void Start()
    {
        _bulletPool = GameManager.GetInstance.GetEnemyBulletPool;
    }

    public void Shoot(Vector3 bulletPos, Vector3 direction, int bulletSpeed)
    {
        GameObject bullet = _bulletPool.GetPooledObject(); // obtiene una bala de la pool
        if (!bullet) return;

        // posiciona la bala
        bullet.transform.position = bulletPos;
        bullet.SetActive(true);

        // impulsa la bala
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
    }
}