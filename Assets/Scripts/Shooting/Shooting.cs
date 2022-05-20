using UnityEngine;

// lo intente hacer medio generico
// para que todo lo que dispares 
// use solo este script llamando
// al metodo Shoot;
public class Shooting : MonoBehaviour
{
    private PoolManager[] _pools = new PoolManager[2]; // este numero es la cantidad de armas que tenes (que disparan)

    private void Start()
    {
        _pools[(int)InventoryManager.ItemID.Bullet] = GameManager.GetInstance.GetBulletPool;
        _pools[(int)InventoryManager.ItemID.Rocket] = GameManager.GetInstance.GetRocketPool;
    }

    public void Shoot(int bulletType, Vector3 bulletPos, Vector3 direction, int bulletSpeed)
    {
        GameObject bullet = _pools[bulletType].GetPooledObject(); // obtiene una bala de la pool
        if (!bullet) return;

        // posiciona la bala
        bullet.transform.position = bulletPos;
        bullet.SetActive(true);

        // impulsa la bala
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
    }
}